using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateMachine; // for state machine


public class MyAnky : Agent
{
    public enum ankyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on y value of the object to denote grass level
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        GRAZING,    // Moving with the intent to find food (will happen after a random period)
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };

    public Animator anim;
    private ankyState state;
    // healt level to be repleinsh while eating grass
    public double health = 100;
    // stamina level to be replenish while drinking water
    public double water = 100;

    public double food = 100;

    // target element for fleeing

    public GameObject target;
    public GameObject goal;

    List<Transform> ankylosaurus; // to store list of Ankies
    List<Transform> targetToFollow; // needed for A*


    //state machine
    public StateMachine<MyAnky> SM;

    public Drinking drinking;
    public Wander wander;
    public Flee flee;
    public FieldOfView view;
    public FieldOfAlert AnkyAlerted;
    public AStarSearch aStar;
    public Herding herding;
    public Grazing grazing;

    void Awake()
    {
        SM = new StateMachine<MyAnky>(this);
        wander = GetComponent<Wander>();
        view = GetComponent<FieldOfView>();
        AnkyAlerted = GetComponent<FieldOfAlert>();
        //AnkyAlerted.viewRadius = 100; // keep as def value
        drinking = GetComponent<Drinking>();
        flee = GetComponent<Flee>();
        aStar = GetComponent<AStarSearch>();
        herding = GetComponent<Herding>();
        grazing = GetComponent<Grazing>();
    }

 
    

    // Use this for initialization

    protected override void Start()
    {
        state = ankyState.IDLE;
        ankylosaurus = new List<Transform>();
        anim = GetComponent<Animator>();

        SM.ChangeState(IdleState.Instance);

        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
    }
    protected override void Update()
    {

        // reduce water lvl over time to mimic the thirst for water
        if (water >= 0)
        {

            water -= (Time.deltaTime * 0.3) * 1;

        }
        // reduce food lvl over time to mimic the hunger
        if (food >= 0)
        {

            food -= (Time.deltaTime * 0.3) * 1;

        }


        // reduce health lvl if Anky don't dring or eat enought
        if (water <= 0 || food <= 0)
        {

            health -= (Time.deltaTime * 0.3) * 1;
        }

        SM.Update();
    }


    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}

