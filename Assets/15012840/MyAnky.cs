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

    public GameObject[] waterArray;

    [Range(0, 100)]
    public int health = 30;

    [HideInInspector]
    public Wander wander;
    [HideInInspector]
    public MyFlee flee;
    [HideInInspector]
    public MyDrinking drinking;
    [HideInInspector]
    public Flee turnback;

    [HideInInspector]
    public FieldOfView view;
    [HideInInspector]
    public FieldOfAlert alerted;
    [HideInInspector]
    public FieldOfGroup herding;
    [HideInInspector]
    public Seek seek;

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
        aS = GetComponent<AStarSearch>();

        wander = GetComponent<Wander>();
        drinking = GetComponent<MyDrinking>();
        flee = GetComponent<MyFlee>();
        turnback = GetComponent<Flee>();
        seek = GetComponent<Seek>();
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
