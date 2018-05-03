using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Statestuff;

public class MyRapty : Agent
{
    public enum raptyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on location of a target object (killed prey)
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        HUNTING,    // Moving with the intent to hunt
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };
    private Animator anim;
    public FieldOfView fov;
    //public Pursue pursueScript;
    public Wander wanderScript;
    public Face facingScript;
    public raptyState currentRaptyState;
    public List<Transform> RaptorsInView = new List<Transform>();
    public List<Transform> AnkyInView = new List<Transform>();
    public GameObject anky;
    public float speed = 2.0f;
    public AStarSearch aStarScript;
    public ASPathFollower pathFollowerScript;

    public GameObject ankyTarget;

    public double hydration = 100;
    public double sustenance = 100;
    public double health = 100;

    public StateMachine<MyRapty> stateMachine { get; set; }

    void Awake()
    {
        fov= GetComponent<FieldOfView>();
        //pursueScript = GetComponent<Pursue>();
        wanderScript = GetComponent<Wander>();
        aStarScript = GetComponent<AStarSearch>();
        pathFollowerScript = GetComponent<ASPathFollower>();
        facingScript = GetComponent<Face>();
        
    }

    // Use this for initialization
    protected override void Start()
    {

        stateMachine = new StateMachine<MyRapty>(this);
        stateMachine.ChangeState(RaptorHuntingState.Instance);
        anim = GetComponent<Animator>();
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        //pursueScript.enabled = false;
        aStarScript.enabled = false;
        pathFollowerScript.enabled = false;
        wanderScript.enabled = false;
        
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
    }

    protected override void Update()
    {
        if(health <=0 && currentRaptyState != raptyState.DEAD)
        {
            stateMachine.ChangeState(RaptorDeadState.Instance);
            currentRaptyState = raptyState.DEAD;
        }
        if (currentRaptyState != raptyState.DEAD)
        {
            desireToLive();
            Search();
            foreach (Transform i in AnkyInView)
            {


                //facingScript.target = i.gameObject;
                anky = new GameObject();
                Vector3 Difference = new Vector3();
                Vector3 ankyDifference = new Vector3();
                Difference = (transform.position - i.position);
                ankyDifference = (transform.position - anky.transform.position);
                if (Difference.magnitude < ankyDifference.magnitude)
                {
                    anky = i.gameObject;
                }
                if (AnkyInView.Count > 0)
                {
                    ankyTarget = anky.gameObject;
                    aStarScript.target = ankyTarget;
                    aStarScript.enabled = true;
                    pathFollowerScript.enabled = true;
                    if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript == null)
                        pathFollowerScript.path = aStarScript.path;
                    move(pathFollowerScript.getDirectionVector());
                }

            }
        }
        stateMachine.Update();
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public void move(Vector3 directionVector)
    {
        directionVector *= speed * Time.deltaTime;

        transform.Translate(directionVector, Space.World);
        transform.LookAt(transform.position + directionVector);
    }

    protected void Search()
    {
        RaptorsInView.Clear();
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Rapty" && !RaptorsInView.Contains(i))
            {
                RaptorsInView.Add(i);
            }
        }

        foreach (Transform i in fov.stereoVisibleTargets)
        {
            if (i.tag == "Rapty" && !RaptorsInView.Contains(i))
            {
                RaptorsInView.Add(i);
            }
        }

        AnkyInView.Clear();
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Anky" && !AnkyInView.Contains(i))
            {           
                    AnkyInView.Add(i);
            }
        }

        foreach (Transform i in fov.stereoVisibleTargets)
        {
            if (i.tag == "Anky" && !AnkyInView.Contains(i))
            {
                    AnkyInView.Add(i);
            }
        }
    }
    
    protected void desireToLive()
    {
        if (hydration > 0)
            hydration -= (Time.deltaTime * 0.2);

        if (sustenance > 0)
            sustenance -= (Time.deltaTime * 0.25);

        if (hydration <= 0 || sustenance <= 0)
        {
            health -= 0.1;
        }

        if (hydration > 85 && sustenance > 85 && health < 100)
        {
            health += 0.2;
        }

    }
   
}
