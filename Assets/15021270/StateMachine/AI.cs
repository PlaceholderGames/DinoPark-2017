using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class AI : MonoBehaviour
{

    public Animator anim;
    
    public Face face;
    public Wander wander;
    public PursueRotate pursue;
    public Agent agent;

    public FieldOfView view;

    public GameObject prey;
    public GameObject friendly;

    public FleeRotate flee;
    public AStarSearch Astar;
    public ASPathFollower follower;

    //Can be changed depending on state
    //Hunting for example takes more energy
    //Eating will stop hunger from being taken away
    public float removeHunger = 1;

    public int distance;
    public float fleeingTime;
    public bool enemy;

    private float hungerTimer; 

    public int hunger = 100;
    public int thirst = 100;
    public int health = 100;

    public bool alpha = false;
    public bool returnToAlpha = false;

    public StateMachine<AI> stateMachine { get; set; }

    void Awake()
    {
        agent = GetComponent<Agent>();
        view = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();
        face = GetComponent<Face>();
        wander = GetComponent<Wander>();
        pursue = GetComponent<PursueRotate>();
        flee = GetComponent<FleeRotate>();
        Astar = GetComponent<AStarSearch>();
        follower = GetComponent<ASPathFollower>();
    }

    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(IdleState.Instance);
    }

    private void Update()
    {
        //if (follower.path.nodes.Count < 0 || follower.path == null)
            //follower.path = Astar.path;

        //move(follower.getDirectionVector());

        //un-specific changes to the raptor such as hunger slowly draining
        hungerTimer += Time.deltaTime;
        if(hungerTimer >= removeHunger)
        {
            hunger -= 1;
            hungerTimer = 0;
            //health -= 10;
            //Debug.Log(health);
        }
        stateMachine.Update();
    }

    public void move(Vector3 directionVector)
    {
        directionVector *= 10 * Time.deltaTime;

        transform.Translate(directionVector, Space.World);
        transform.Rotate(transform.position + directionVector);
    }

}