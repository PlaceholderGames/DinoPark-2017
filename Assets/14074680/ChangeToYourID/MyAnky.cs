using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using stateDino;

public class MyAnky : Agent
{

    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;
    public double hydration = 100;
    public double food = 100;
    public double health = 100;
    public GameObject Water;
    public GameObject Food;
    public Seek AnkySeek;
    public Seek AnkyFood;
    public Wander AnkyWander;
    public Flee AnkyFlee;
    public List<Transform> AnkyVillains = new List<Transform>();
    public List<Transform> AnkyHeroes = new List<Transform>();
    public FieldOfView FOV;

    public StateMachine<MyAnky> StateMachine { get; set; }

    public int Distance { get; internal set; }

    void Awake()
    {
        AnkyWander = GetComponent<Wander>();
        AnkySeek = GetComponent<Seek>();
        AnkyFood = GetComponent<Seek>();
        AnkyFlee = GetComponent<Flee>();
        FOV = GetComponent<FieldOfView>();
    }
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

    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        StateMachine = new StateMachine<MyAnky>(this);
        StateMachine.ChangeState(IdleState.Instance);
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
        AnkyFood.target = Food;
        base.Start();

    }



    protected override void Update()
    {
        if (Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            seconds++;
        }

        if (seconds == 5)
        {
            if (food >= 1)
            {
                food--;
            }
            if (hydration >= 1)
            {
                hydration--;
            }
            seconds = 0;
            //switchState = !switchState;

            if (hydration <= 0)
            {
                if (health >= 1)
                {
                    health--;
                }
            }

            if (food <= 0)
            {
                if (health >= 1)
                {
                    health--;
                }
            }

            if (health <= 1)
            {
                StateMachine.ChangeState(DeadState.Instance);
            }

        }
      AnkyVillains.Clear();

            foreach (Transform j in FOV.visibleTargets)
            {
                if (j.tag == "Rapty" )
                {
                    AnkyVillains.Add(j);
                }
            }

            foreach (Transform j in FOV.stereoVisibleTargets)
            {
                if (j.tag == "Rapty")
                {
                    AnkyVillains.Add(j);
                }
            }
        StateMachine.Update();
        base.Update();

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
