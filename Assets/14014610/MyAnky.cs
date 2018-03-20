using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Statestuff;

public class MyAnky : Agent
{

    public bool switchState = false;
    public StateMachine<MyAnky> stateMachine { get; set; }

    public List<Transform> enemies = new List<Transform>();
    public List<Transform> friendlies = new List<Transform>();
    public FieldOfView fov;
    public Wander ankyWander;
    public Flee ankyFlee;

    public double hydration = 100;

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
    public ankyState currentAnkyState;


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

        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(grazingState.Instance);


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

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        foreach (Transform target in fov.visibleTargets)
        {
            if (target.tag == "Rapty" && !enemies.Contains(target))
            {
                enemies.Add(target);
            }
        }
        foreach (Transform target in fov.stereoVisibleTargets)
        {
            if (target.tag == "Rapty" && !enemies.Contains(target))
            {
                enemies.Add(target);
            }
        }

        hydration -= (Time.deltaTime * 0.2) * 1;
        stateMachine.Update();
        base.Update();
    }


    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
