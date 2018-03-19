using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

    [Range(0, 100)]
    public int health = 30;

    Wander wander;
    Flee flee;
    Drinking drinking;
    
    FieldOfView view;
    GameObject target;
    // Use this for initialization
    void Awake()
    {
        view = GetComponent<FieldOfView>();
        wander = GetComponent<Wander>();
        drinking = GetComponent<Drinking>();
        flee = GetComponent<Flee>();
    }
    protected override void Start()
    {
        state = ankyState.IDLE;
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

    protected override void Update()
    {
        TransformToState();
        // Idle - should only be used at startup
        if(state.ToString() == "IDLE")
        {
            TurnOffTheScripts();
            wander.enabled = true;
        }
        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)
        if (state.ToString() == "DRINKING")
        {
            flee.enabled = false;
            drinking.enabled = true;
            drinking.Drink();
        }
        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here
        if (state.ToString() == "FLEEING")
        {
            wander.enabled = false;
            //TurnOffTheScripts();
            flee.target = target;
            flee.enabled = true;
        }
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }

    private void TransformToState()
    {
        TransformToFleeing();
        TransformToDrinking();
        //idl
        //eating
    }

    private void TransformToDrinking()
    {
        if(transform.position.y < 35)
        {
            state = ankyState.DRINKING;
        }
    }

    private void TransformToFleeing()
    {
        foreach (Transform animal in view.visibleTargets)
        {
            if(animal.tag == "Rapty")
            {
                state = ankyState.FLEEING;
                target = animal.gameObject;
            }
        }
    }

    private void TurnOffTheScripts()
    {
        wander.enabled = false;
        flee.enabled = false;
        drinking.enabled = false;
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
