using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Statestuff;
 
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

    public float gameTimer;
    public float seconds = 0;
    public bool switchState = false;
    public int Vigor = 100;
    public int Hunger = 100;
    public int Thirst = 100;
    public Seek AnkySeek;
    public Seek FoodSeek;
    public GameObject Water;
    public GameObject Food;
    public Wander AnkyWander;
    public Flee AnkyFlee;
    public FieldOfView FOV;
    public List<Transform> Enemies = new List<Transform>();

    public StateMachine<MyAnky> stateMachine { get; set; }
    public int Enemydistance { get; internal set; }

    public Animator anim;

    void Awake()
    {
        AnkyWander = GetComponent<Wander>();
        AnkySeek = GetComponent<Seek>();
        FoodSeek = GetComponent<Seek>();
        AnkyFlee = GetComponent<Flee>();
        FOV = GetComponent<FieldOfView>();
    }

    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(GrazingState.Instance);
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
        AnkySeek.target = Water;
        FoodSeek.target = Food;
        base.Start();
    }

    protected override void Update()
    {
        if (Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            seconds++;
            Debug.Log(seconds);
        }

        if (seconds == 5)
        {
            if (Hunger >= 1)
            {
                Hunger--;
            }

            if (Thirst >= 1)
            {
                Thirst--;
            }

            seconds = 0;

            if (Thirst <= 0)
            {
                if (Vigor >= 1)
                {
                    Vigor--;
                }
            }

            if (Hunger <= 0)
            {
                if (Vigor >= 1)
                {
                    Vigor--;
                }
            }

            if (Vigor <= 1)
            {
                stateMachine.ChangeState(DeadState.Instance);
            }    
        }

        Enemies.Clear();

            foreach (Transform i in FOV.visibleTargets)
            {
                if (i.tag == "Rapty")
                {
                    Enemies.Add(i);
                }
            }

            foreach (Transform i in FOV.stereoVisibleTargets)
            {
                if (i.tag == "Rapty")
                {
                    Enemies.Add(i);
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