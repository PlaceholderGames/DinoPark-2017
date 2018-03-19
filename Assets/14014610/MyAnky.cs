using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAnky : Agent
{
	public FieldOfView fov;
    public Wander ankyWander;
    public Flee ankyFlee;

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
    private ankyState currentState;


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

    }
    void Awake()
    {
        ankyWander = GetComponent<Wander>();
        ankyFlee = GetComponent<Flee>();
    }
    protected override void Update()
    {
        // Idle - should only be used at startup
        
        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)
        if(this.transform.position.y < 32)
        {
            currentState = ankyState.DRINKING;
        }


        // Alerted - up to the student what you do here
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Rapty" && Vector3.Distance(i.position, this.transform.position) > 60)
            {
                currentState = ankyState.ALERTED;
                anim.SetBool("isAlerted", true);
            }
            else { anim.SetBool("isAlerted", false); }
        }
        
        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Rapty" && Vector3.Distance(i.position, this.transform.position) < 30)
            {
                currentState = ankyState.FLEEING;
                ankyFlee.target = i.gameObject;
                anim.SetBool("isFleeing", true);
            }
            else { anim.SetBool("isFleeing", false); }
        }


        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed


        if (currentState == ankyState.ALERTED)
        {
            ankyWander.enabled = true;
            ankyFlee.enabled = false;
        }
        if (currentState == ankyState.FLEEING)
        {
            ankyFlee.enabled = true;
            ankyWander.enabled = false;
        }


        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
