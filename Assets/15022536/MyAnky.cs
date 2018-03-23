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
    public int health = 40;
    // stamina level to be replenish while drinking water
    public int stamina = 40;
    // target element for fleeing
    public GameObject target;


    //state machine
    public StateMachine<MyAnky> stateMachine { get;set; }

    public DrinkingState drinking;
    public Wander wander;
   // public FleeState flee;
    public FieldOfView AnkyView;

    void Awake()
    {
        wander = GetComponent<Wander>();
        AnkyView = GetComponent<FieldOfView>();
        drinking = GetComponent<DrinkingState>();
       // flee = GetComponent<Flee>();
    }

    private void ChangeState()
    {
        ChangeToDrinking();
        ChangeToFlee();
    }

    private void ChangeToDrinking()
    {
        if (transform.position.y < 32 && this.stamina < 50)
        {
            state = ankyState.DRINKING;
        }
    }

    private void ChangeToEating()
    {
        if (this.health < 50 && this.stamina < 50)
        {
            // go find grass

            // state = ankyState.EATING;
        }

    }


    private void ChangeToWander()
    {
        wander.enabled = true;
    }

    private void ChangeToFlee()
    {
        foreach(Transform animal in AnkyView.visibleTargets)
        {
            if(animal.tag == "Rapty")
            {
                state = ankyState.FLEEING;
                target = animal.gameObject;
            }
        }
        if (AnkyView.visibleTargets.Count == 0)
        {
            // change it to grazing later so the anky would seek grass rather than stand around
            state = ankyState.IDLE;
        }
    }
    // Use this for initialization

    protected override void Start()
    {
        state = ankyState.IDLE;
        ankylosaurus = new List<Transform>();
        anim = GetComponent<Animator>();

        mySM.ChangeState(MyIdleState.Instance);

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
        mySM.Update();
    }


    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}

