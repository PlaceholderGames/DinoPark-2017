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
    public bool switchState = false;
    public StateMachine<MyAnky> stateMachine { get;set; }

   

    Wander wander;
    FieldOfView AnkyView;
    DrinkSC drink;
    Flee flee;

    
    void Awake()
    {
        wander = GetComponent<Wander>();
        AnkyView = GetComponent<FieldOfView>();
        drink = GetComponent<DrinkSC>();
        flee = GetComponent<Flee>();
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
        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(FirstState.Instance);
        
    }
    

  
    protected override void Update()
    {

        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        stateMachine.Update();
       //  base.Update();
    }

    // previouse version
    /*
    protected override void Update()
    {
        ChangeState();

        // Idle - should only be used at startup

        if (state.ToString() == "IDLE")
        {
            Debug.Log("idle");
        }
        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32
        else if (state.ToString() == "DRINKING")
        {
            Debug.Log("drink");
            flee.enabled = false;

            drink.enabled = true;
            drink.Drink();

        }



        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here
        else if (state.ToString() == "FLEEING")
        {
            Debug.Log("flee");
            wander.enabled = false;
            drink.enabled = false;
            flee.enabled = true;
            flee.target = target;
        }
        if (state.ToString() == "ALERTED")
        {
            Debug.Log("alerted");
        }

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }
    */

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}

