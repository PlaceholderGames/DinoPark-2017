using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class MyAnky : Agent
{
    public enum ankyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on y value of the object to denote grass level
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        HERDING,
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

    [HideInInspector]
    public Wander wander;
    [HideInInspector]
    public MyFlee flee;
    [HideInInspector]
    public MyDrinking drinking;

    [HideInInspector]
    public FieldOfView view;
    [HideInInspector]
    public FieldOfAlert alerted;
    [HideInInspector]
    public FieldOfGroup herding;
    bool herdingBool = false;

    List<Transform> follow;
    List<Transform> ankies;
    public GameObject goal;

    public StateMachine<MyAnky> mySM;
    //public GameObject target;
    [HideInInspector]
    public AStarSearch aS;
    // Use this for initialization
    void Awake()
    {
        mySM = new StateMachine<MyAnky>(this);
        view = GetComponent<FieldOfView>();
        alerted = GetComponent<FieldOfAlert>();
        herding = GetComponent<FieldOfGroup>();
        alerted.viewRadius = 100;
        wander = GetComponent<Wander>();
        drinking = GetComponent<MyDrinking>();
        flee = GetComponent<MyFlee>();
        aS = GetComponent<AStarSearch>();
    }
    protected override void Start()
    {
        state = ankyState.IDLE;
        ankies = new List<Transform>();
        anim = GetComponent<Animator>();

        mySM.ChangeState(MyIdleState.Instance);

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
        mySM.Update();
    }
/*
    protected override void Update()
    {
        TransformToState();

        // Idle - should only be used at startup
        if (state.ToString() == "IDLE")
        {
            ResetTheScripts();
        }
        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)
        else if (state.ToString() == "DRINKING")
        {
            Debug.Log("drink");
            drinking.enabled = true;
            drinking.Drink();
        }
        // Hunting - up to the student what you do here
        // Fleeing - up to the student what you do here
        else if (state.ToString() == "FLEEING")
        {
            flee.enabled = true;
            Debug.Log("flee");
            wander.enabled = false;
        }
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
        else if (state.ToString() == "HERDING")
        {
            flee.enabled = true;
            wander.enabled = false;
            Debug.Log(view.visibleTargets.Count);
            if (view.visibleTargets.Count > 1)
            {
                Debug.Log("here");
                state = ankyState.IDLE;
                flee.enabled = false;
                wander.enabled = false;
                return;
            }
        }
        if (state.ToString() == "ALERTED")
        {
        }
        base.Update();
    }
*/
    private void TransformToState()
    { 
    }

    private void TransformToAlert()
    {
        foreach (Transform animal in alerted.visibleTargets)
        {
            if (animal.tag == "Rapty" && state.ToString() != "FLEEING")
            {
                Debug.Log("Alerted");
                state = ankyState.ALERTED;
            }
        }
    }

    private void TransformToDrinking()
    {
        if (transform.position.y < 35 && health < 80 && state.ToString() != "FLEEING")
        {
            state = ankyState.DRINKING;
        }
    }

    private void TransformToFleeing()
    {
        if (state.ToString() != "HERDING")
        {
            foreach (Transform animal in view.visibleTargets)
            {
                if (animal.tag == "Rapty")
                {
                    Debug.Log("Fleeing");
                    aS = GetComponent<AStarSearch>();
                    state = ankyState.FLEEING;
                    aS.target = goal;
                }
            }
            if (alerted.visibleTargets.Count > 1)
            {
                foreach (Transform animal in alerted.visibleTargets)
                {
                    if (animal.tag == "Rapty")
                        return;
                }
                Debug.Log("Back to idle From fleeing");
                state = ankyState.IDLE;
            }
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
