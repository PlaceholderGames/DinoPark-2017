using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateMachineBase;

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
    public FieldOfView fov;

    public bool switchState = false;
    public float gameTimer;
    public int secondsHunger = 0;
    public int secondsThirst = 0;
    public int ankyHealth = 100;
    public int ankyThirst = 5;
    public int ankyHunger = 5;
    public Flee ankyFleeing;
    public List<Transform> ankyEnemies = new List<Transform>();
    public Wander ankyWandering;

    public StateMachine<MyAnky> stateMachine { get; set; }

    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();
        ankyFleeing = GetComponent<Flee>();
        ankyWandering = GetComponent<Wander>();
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
        stateMachine.ChangeState(StateBeginning.Instance);

        gameTimer = Time.time;

        Debug.Log("Started Anky!");

        base.Start();

    }

    protected override void Update()
    {

        ankyEnemies.Clear();

        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Rapty")
            {
                ankyEnemies.Add(i);

            }
        }

        /*
        foreach (Transform i in fov.stereoVisibleTargets)
        {
            if (i.tag == "Rapty")
            {
                ankyEnemies.Add(i);

            }
        }
        */

        if (Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            secondsHunger++;
            secondsThirst++;
        }

        if (secondsHunger == 1)
        {
            if (ankyHunger <= 0)
            {
                Debug.Log("Anky is starving...");
                ankyHealth--;
            }

            if (ankyThirst <= 0)
            {
                Debug.Log("Anky is dehydrated...");
                ankyHealth--;
            }

            if (100-ankyHealth <= 0)
            {
                if (stateMachine.currentState != StateDead.Instance)
                {
                    Debug.Log("Anky has died!");
                    stateMachine.ChangeState(StateDead.Instance);
                }
            }
        }

        if (stateMachine.currentState != StateEating.Instance)
        {
            if (secondsHunger == 2)
            {

                if (ankyHunger >= 1)
                {
                    ankyHunger--;
                    secondsHunger = 0;
                }

            }
        }

        if (stateMachine.currentState != StateDrinking.Instance)
        {
            if (secondsThirst == 5)
            {
                if (ankyThirst >= 1)
                {
                    ankyThirst--;
                    secondsThirst = 0;
                }

            }
        }

        stateMachine.Update();
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
