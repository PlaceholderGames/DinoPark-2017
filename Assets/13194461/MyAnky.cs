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

    ankyState currentState;
    ankyState previousState;
    public Animator anim;

    // Use this for initialization
    protected override void Start()
    {

        anim = GetComponent<Animator>();
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", true);
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
        // Idle - should only be used at startup

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

    void SetState()
    {
        switch(currentState)
        {
            case ankyState.EATING:
                anim.SetBool("isEating", true);
                break;
            case ankyState.DRINKING:
                anim.SetBool("isDrinking", true);
                break;
            case ankyState.ALERTED:
                anim.SetBool("isAlerted", true);
                break;
            case ankyState.GRAZING:
                anim.SetBool("isGrazing", true);
                break;
            case ankyState.ATTACKING:
                anim.SetBool("isAttacking", true);
                break;
            case ankyState.FLEEING:
                anim.SetBool("isFleeing", true);
                break;
            case ankyState.DEAD:
                anim.SetBool("isDead", true);
                break;
        }

        switch(previousState)
        {
            case ankyState.IDLE:
                anim.SetBool("isIdle", false);
                break;
            case ankyState.EATING:
                anim.SetBool("isEating", false);
                break;
            case ankyState.DRINKING:
                anim.SetBool("isDrinking", false);
                break;
            case ankyState.ALERTED:
                anim.SetBool("isAlerted", false);
                break;
            case ankyState.GRAZING:
                anim.SetBool("isGrazing", false);
                break;
            case ankyState.ATTACKING:
                anim.SetBool("isAttacking", false);
                break;
            case ankyState.FLEEING:
                anim.SetBool("isFleeing", false);
                break;
            case ankyState.DEAD:
                anim.SetBool("isDead", false);
                break;
        }
    }
}
