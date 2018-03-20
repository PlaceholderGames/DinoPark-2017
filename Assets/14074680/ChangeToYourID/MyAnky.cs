using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using stateDino;

public class MyAnky : Agent
{

    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;

    public StateMachine<MyAnky> StateMachine { get; set; }

    private void Start()
    {
        StateMachine = new StateMachine<MyAnky>(this);
        StateMachine.ChangeState(DrinkingState.Instance);
        gameTimer = Time.time;
    }

    private void Update()
    {
        if (Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            seconds++;
            Debug.Log(seconds);
        }

        if (seconds == 5)
        {
            seconds = 0;
            switchState = !switchState;
        }

        StateMachine.Update();
    }


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

    // Use this for initialization
    //protected override void Start()
    //{
    //    anim = GetComponent<Animator>();
    //    // Assert default animation booleans and floats
    //    anim.SetBool("isIdle", true);
    //    anim.SetBool("isEating", false);
    //    anim.SetBool("isDrinking", false);
    //    anim.SetBool("isAlerted", false);
    //    anim.SetBool("isGrazing", false);
    //    anim.SetBool("isAttacking", false);
    //    anim.SetBool("isFleeing", false);
    //    anim.SetBool("isDead", false);
    //    anim.SetFloat("speedMod", 1.0f);
    //    // This with GetBool and GetFloat allows 
    //    // you to see how to change the flag parameters in the animation controller
    //    base.Start();

    //}



    //protected override void Update()
    //{
    //    // Idle - should only be used at startup

    //    // Eating - requires a box collision with a dead dino

    //    // Drinking - requires y value to be below 32 (?)

    //    // Alerted - up to the student what you do here

    //    // Hunting - up to the student what you do here

    //    // Fleeing - up to the student what you do here
    //    // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
    //}

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
