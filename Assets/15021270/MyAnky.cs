using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public Wander wander;
    public FieldOfView view;
    public Flee flee;
    public ankyState currentState;

    // Use this for initialization
    protected override void Start()
    {
        //anim = GetComponent<Animator>();
        wander = GetComponent<Wander>();
        view = GetComponent<FieldOfView>();
        flee = GetComponent<Flee>();
        currentState = GetComponent<ankyState>();

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


        currentState = ankyState.IDLE;

        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();

    }

    protected override void Update()
    {
        // Idle - should only be used at startup
        if (currentState == ankyState.IDLE)
        {
            wander.enabled = true;
            flee.enabled = false;
            rapterCheck();
        }
        else if (currentState == ankyState.EATING)
        {
            
        }
        else if (currentState == ankyState.DRINKING)
        {
        }
        else if (currentState == ankyState.ALERTED)
        {
        }
        else if (currentState == ankyState.GRAZING)
        {
        }
        else if (currentState == ankyState.ATTACKING)
        {
        }
        else if (currentState == ankyState.FLEEING)
        {
            wander.enabled = false;
            flee.enabled = true;
        }
        else if (currentState == ankyState.DEAD)
        {
        }
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

    protected void rapterCheck()
    {
        foreach (Transform i in view.visibleTargets)
        {
            if (i.tag == "Rapty")
            {
                currentState = ankyState.FLEEING;
                flee.target = i.gameObject;
            }
        }
    }
}
