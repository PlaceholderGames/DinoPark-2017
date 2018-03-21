using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class AI : MonoBehaviour
{

    public Animator anim;
    public FieldOfView view;
    public Face face;
    public Wander wander;
    public PursueRotate pursue;
    public Agent agent;
    public GameObject prey;
    public FleeRotate flee;

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
    }

    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(IdleState.Instance);
    }

    private void Update()
    {
        //un-specific changes to the raptor such as hunger slowly draining
        hungerTimer += Time.deltaTime;
        if(hungerTimer >= removeHunger)
        {
            hunger -= 1;
            hungerTimer = 0;
            health -= 10;
            Debug.Log(health);
        }
        stateMachine.Update();
    }
}