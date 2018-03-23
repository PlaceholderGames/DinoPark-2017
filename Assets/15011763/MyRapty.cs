using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyRapty : Agent
{
    public enum raptyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on location of a target object (killed prey)
        DRINKING,   // This is for DrinkingState, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        HUNTING,    // Moving with the intent to hunt
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };
    private Animator myAnim;

    // Use this for initialization
    protected override void Start()
    {
        //myAnim = GetComponent<Animator>();
        //// Assert default animation booleans and floats
        //myAnim.SetBool("isIdle", true);
        //myAnim.SetBool("isEating", false);
        //myAnim.SetBool("isDrinking", false);
        //myAnim.SetBool("isAlertedStateed", false);
        //myAnim.SetBool("isHunting", false);
        //myAnim.SetBool("isAttacking", false);
        //myAnim.SetBool("isFleeing", false);
        //myAnim.SetBool("isDead", false);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
    }

    protected override void Update()
    {
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // DrinkingState - requires y value to be below 32 (?)

        // AlertedStateed - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'hungryDino' until it is consumed

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
