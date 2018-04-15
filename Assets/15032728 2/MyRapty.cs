using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    raptyState current;   //Create a variable representing the existing state.

    // Use this for initialization
    protected override void Start()
    {
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
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
    }

    protected override void Update()
    {
        // Idle - should only be used at startup
        current = raptyState.IDLE;

        // Eating - requires a box collision with a dead dino
        current = raptyState.EATING;

        // Drinking - requires y value to be below 32 (?)
        current = raptyState.DRINKING;

        // Alerted - up to the student what you do here
        current = raptyState.ALERTED;

        // Hunting - up to the student what you do here
        current = raptyState.HUNTING;

        // Fleeing - up to the student what you do here
        current = raptyState.FLEEING;

        // Dead - If the animal is being eaten, reduce its 'health' until it's consumed
        current = raptyState.DEAD;

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    //Functions for the states that are going to be used.
    // Idle - should only be used at startup
    void Idle()
    {
        anim.SetBool("isIdle", true);
    }
    // Eating - requires a box collision with a dead dino
    void Eating()
    {
        anim.SetBool("isEating", false);
        agent.Eat();
    }
    // Drinking - requires y value to be below 32 (?)
    void Drinking()
    {
        if(y>32)
        {
        anim.SetBool("isDrinking", false);
        agent.Drink();
        }
    }
    // Alerted - up to the student what you do here
    void Alerted()
    {
        anim.SetBool("isAlerted", false);
        agent.Awaken();
    }
    // Hunting - up to the student what you do here
    void Hunting()
    {
        anim.SetBool("isHunting", false);
        agent.Hunt();
    }
    // Fleeing - up to the student what you do here
    void Fleeing()
    {
        anim.SetBool("isFleeing", false);
        agent.Flee();
    }
    // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
    void Dead()
    {
        anim.SetBool("isDead", false);
    }
}
