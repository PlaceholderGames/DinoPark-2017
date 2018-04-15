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
    private int health = 0;     //So that it will be known when anky dies.
    private int y = 0;      //Height of the map.
    ankyState current;   //Create a variable representing the existing state.

    // Use this for initialization
    protected override void Start()
    {
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

        current = ankyState.IDLE;   //Initialise.
    }

    protected override void Update()
    {
        // Idle - should only be used at startup
        current = ankyState.IDLE;

        // Eating - requires a box collision with a dead dino
        current = ankyState.EATING;

        // Drinking - requires y value to be below 32 (?)
        current = ankyState.DRINKING;

        // Alerted - up to the student what you do here
        current = ankyState.ALERTED;

        // Grazing - up to the student what you do here
        current = ankyState.GRAZING;

        // Fleeing - up to the student what you do here
        current = ankyState.FLEEING;

        // Dead - If the animal is being eaten, reduce its 'health' until it's consumed
        current = ankyState.DEAD;

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
    // Should be eating grass for an anky, rather than carrion?
    // - requires y value to be below 32 (?)
    void Eating()
    {
        if(y>32)
            {
             anim.SetBool("isEating", true);
             anim.SetBool("isFleeing", false);
             anim.SetBool("isDrinking", false);
             anim.SetBool("isGrazing", false);
             agent.Eat();
            }
    }


    // Drinking - requires y value to be below 32 (?)
    void Drinking()
    {
        if(y>32)
        {
        anim.SetBool("isDrinking", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isGrazing", false);
        }
    }
    // Alerted - up to the student what you do here
    void Alerted()
    {
        anim.SetBool("isAlerted", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isDrinking", false);
        AgentAwared.MonoBehaviour();   //Run the AgentAwared script in Agent -> AgentAwareness
    }
    // Grazing - up to the student what you do here
    void Grazing()  //This is searching for food according to above
    {
        anim.SetBool("isGrazing", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDrinking", false);
        Wander.Face();   //Run the Wander script in Agent -> Behaviours
    }
    // Fleeing - up to the student what you do here
    void Fleeing()
    {
        anim.SetBool("isFleeing", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isGrazing", false);
        AgentBehaviour.Flee();   //Run the flee script in Agent -> Behaviours
    }
    // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
    void Dead()
    {
        anim.SetBool("isDead", true);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        Agent.health = 0;
    }
}
