using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using stateMachine2;

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

    

    public bool changeState = false;
    public double dinoTimer;
    public double seconds = 0;
    public double ankyHealth = 100;
    public double ankyHydration = 100;
    public double ankyAppetite = 100;
    public FieldOfView FOV;
    public GameObject Water;
    public GameObject Food;
    public GameObject rapty = null;
    public Vector3 Distance = new Vector3();
    public Seek seekScript;
    public Wander wanderScript;
    public Flee fleeScript;
    public List<Transform> raptyInFOV = new List<Transform>();
    public Transform ankyPosition;


    public Behaviour<MyAnky> stateMachine { get; set; }
    public Animator anim;

    // Use this for initialization

    void Awake()
    {
        fleeScript = GetComponent<Flee>();
        wanderScript = GetComponent<Wander>();
        seekScript = GetComponent<Seek>();
        FOV = GetComponent<FieldOfView>();
    }

    public void FieldOfView()
    {
        raptyInFOV = new List<Transform>();
        Vector3 VisableDistance = new Vector3(1200, 1200, 1200);
        int index = 0;

        foreach (Transform i in FOV.visibleTargets)
        {
            if(i.tag == "Rapty" && !raptyInFOV.Contains(i))
            {
                raptyInFOV.Add(i);
            }
        }

        raptyInFOV.Clear();

        foreach (Transform i in FOV.stereoVisibleTargets)
        {
            if(i.tag == "Rapty" && !raptyInFOV.Contains(i))
            {
                raptyInFOV.Add(i);
            }
        }

        rapty = null;

        for (int i = 0; i < FOV.visibleTargets.Count; i++)
        {
            Distance = (transform.position - raptyInFOV[i].position);

            if (Distance.magnitude < VisableDistance.magnitude)
            {
                VisableDistance = Distance;
                index = i;
                rapty = raptyInFOV[index].gameObject;
            }
        }
    }
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

        stateMachine = new Behaviour<MyAnky>(this);
        stateMachine.changeState(grazingState.Instance);

        dinoTimer = Time.time;

        seekScript.target = Water;

        base.Start();

    }

    protected override void Update()
    {
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino
        if (ankyAppetite >= 0)
        {
            ankyAppetite -= (Time.deltaTime * 0.3) * 1;
        }

        if (ankyAppetite <= 0)
        {
            ankyHealth -= (Time.deltaTime * 0.4) * 1;
        }

        // Drinking - requires y value to be below 32 (?)
        if (ankyHydration >= 0)
        {
            ankyHydration -= (Time.deltaTime * 0.3) * 1;
        }

        if (ankyHydration <= 0)
        {
            ankyHealth -= (Time.deltaTime * 0.4) * 1;
        }
        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
        stateMachine.Update();
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
