using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateMachineBase;

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
    public List<Transform> raptyTargets = new List<Transform>();
    public Wander raptyWander;

    public StateMachine<MyRapty> stateMachine { get; set; }


    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();
        raptyWander = GetComponent<Wander>();

        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller

        stateMachine = new StateMachine<MyRapty>(this);
        stateMachine.ChangeState(StateBeginningRapty.Instance);


        Debug.Log("Started Rapty!");
        base.Start();
    }

    protected override void Update()
    {
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        stateMachine.Update();
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
