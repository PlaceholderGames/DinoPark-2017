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
    private FieldOfView view;
    private Face face;
    private Wander wander;
    private Pursue pursue;
    public raptyState currentState;
    public int health;

    // Use this for initialization
    void Awake()
    {
        view = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();
        face = GetComponent<Face>();
        wander = GetComponent<Wander>();
        pursue = GetComponent<Pursue>();
    }

    protected override void Start()
    {
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);

        currentState = raptyState.IDLE;

        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
    }

    protected override void Update()
    {
        if (currentState == raptyState.IDLE)
        {
            checkForAnky();
            //wander.enabled = true;
            Debug.Log("idle!");
        }
        else if (currentState == raptyState.EATING)
        {

        }
        else if (currentState == raptyState.DRINKING)
        {

        }
        else if (currentState == raptyState.ALERTED)
        {

        }
        else if (currentState == raptyState.HUNTING)
        {

            wander.enabled = false;
            face.enabled = true;
            pursue.enabled = true;

            this.SetSteering(face.GetSteering());
            this.SetSteering(pursue.GetSteering());
            Debug.Log("Hunting!");
            
        }
        else if (currentState == raptyState.ATTACKING)
        {

        }
            // Idle - should only be used at startup

            // Eating - requires a box collision with a dead dino

            // Drinking - requires y value to be below 32 (?)

            // Alerted - up to the student what you do here

            // Hunting - up to the student what you do here


            // Fleeing - up to the student what you do here

            // Dead - If the animal is being eaten, reduce its 'health' until it is consumed



            base.Update();
    }



    private void checkForAnky()
    {
        foreach (Transform i in view.visibleTargets)
        {
            if (i.tag == "Anky")
            {
                currentState = raptyState.HUNTING;
            }
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
