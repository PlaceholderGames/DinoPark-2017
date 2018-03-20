using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Statestuff;
public class MyAnky : Agent
{
    public FieldOfView fov;
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
    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;
    public StateMachine<MyAnky> stateMachine { get; set; }

    // Use this for initialization
    protected override void Start()
    {
        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(IdleState.Instance);
        gameTimer = Time.time;

        anim = GetComponent<Animator>();
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

    // Update is called once per frame
    protected override void Update()
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
        stateMachine.Update();
        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
