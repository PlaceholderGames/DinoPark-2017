using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateStuff;



public class MyRapty : Agent
{
    public StateMachine<MyRapty> raptyMachine { get; set; }
    public GameObject closestAnky;
    public GameObject closestWater;
    public int dinoCheck;
    public bool Dead = false;
    public bool anotherDead = false;
    float weight = 1.0f;
    public int health = 100;
    public int fleeingHealth = 20;
    public float hunger = 100.0f;
    public float thirstLevel = 100.0f;
    public Pursue myPursue;
    public FieldOfView myFOV;
    public Wander myWander;
    private MyAnky ankyRef;
    public MyRapty rappydoos;
    public AStarSearch myA_star;
    public ASAgentInstance myAS_instance;
    public ASPathFollower myAS_pathFollower;
    public Agent myAgent;
    public List<Transform> waterList;
    public DownsideUpScript myDownsideUp;
    public enum raptyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on location of a target object (killed prey)
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        HUNTING,    // Moving with the intent to hunt
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD,
        SEARCHING,
        EXITING
    };
    public Animator anim;
    
    // Use this for initialization


    protected override void Start()
    {
        myAgent = GetComponent<Agent>();
        myWander = GetComponent<Wander>();
        myPursue = GetComponent<Pursue>();
        anim = GetComponent<Animator>();
        myFOV = GetComponent<FieldOfView>();
        myA_star = GetComponent<AStarSearch>();
        myAS_pathFollower = GetComponent<ASPathFollower>();
        myAS_instance = GetComponent<ASAgentInstance>();
        myDownsideUp = GetComponent<DownsideUpScript>();
        raptyMachine = new StateMachine<MyRapty>(rappydoos);
        //raptyMachine.ChangeState(IdleState.Instance);
        myPursue.enabled = false;

       


        raptyMachine.ChangeState(SearchingState.Instance);
        if (!anim)
        {
            Debug.Log("This is a problem");
        }


        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetBool("isSearching", false);
        anim.SetBool("isExiting", false);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller

        StartCoroutine(Dying());
        base.Start();
    }

    protected override void Update()
    {
        if (!Dead)
        {
            raptyMachine.Update();
        }    
       else
        {
            raptyMachine.ChangeState(DeadState.Instance);
        }

        base.Update();
    }

    

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }




    IEnumerator Dying()
    {        
        if (!Dead)
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);
                Debug.Log("Thirst level:" + thirstLevel);
                Debug.Log("Hunger :" + hunger);
                Debug.Log("Health " + health);
                Debug.Log("is alive?" + Dead);
                thirstLevel = thirstLevel - 1.0f;
                hunger = hunger - 0.33f;
                if (hunger <= 0)
                {
                    hunger = 0;
                }
                if (thirstLevel <= 0)
                {
                    thirstLevel = 0;
                }
                if (thirstLevel == 0 && hunger == 0)
                {
                    health = health - 3;
                }
                else if (thirstLevel == 0 || hunger == 0)
                {
                    health = health - 1;
                }
                if (health <= 0)
                {
                    Dead = true;
                    health = 0;
                }
                if (Dead)
                {
                    myDownsideUp.DownsideUpMeBro(rappydoos);
                    break;
                }                  
            }
        }
       
          


        

       
    }
}
