using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Statestuff;
public class MyAnky : Agent
{
    public FieldOfView fov;
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
    public Flee ankyFlee;
    public Wander ankyWander;
    public Seek ankySeek;
    public GrassScript GrassScript;
    //public AStarSearch ankyAS;
    //public ASPathFollower ankyPath;
    public bool switchState = false;
    public double saturation = 100;
    public double hydration = 100;
    public double health = 100;
    public float gameTimer;
    public int seconds = 0;
    public GameObject Water;
    public List<Transform> Enemies = new List<Transform>();
    public List<Transform> friends = new List<Transform>();
    

    public StateMachine<MyAnky> stateMachine { get; set; }


    //void Awake()
    //{
    //    ankyFlee = GetComponent<Flee>();
    //    ankyWander = GetComponent<Wander>();
    //    fov = GetComponent<FieldOfView>();

    //}


    // Use this for initialization
    protected override void Start()
    {
        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(IdleState.Instance);
        gameTimer = Time.time;

        anim = GetComponent<Animator>();
        //ankyFlee = GetComponent<Flee>();
        //ankyWander = GetComponent<Wander>();
        //ankySeek = GetComponent<Seek>();
        //fov = GetComponent<FieldOfView>();
        //ankyAS = GetComponent<AStarSearch>();
        //ankyPath = GetComponent<ASPathFollower>();
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

        

        ankySeek.target = Water;
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
      
    }

    public bool takeDamage(int damage)
    {
        health -= damage;
     
        return false;
    }

    // Update is called once per frame
    protected override void Update()
    {
       

       
        // Eating - Drinking 
        if (hydration > 0)
        {
            hydration -= (Time.deltaTime * 0.2) * 1.0;
        }
        if (saturation > 0)
        {
            saturation -= (Time.deltaTime * 0.1) * 1.0;
        }

        if (hydration <= 0 || saturation <=0 || health <= 0)
        {

            stateMachine.ChangeState(DeadState.Instance);
        }
        

        // Alerted - up to the student what you do here
        Enemies.Clear();
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Rapty")
            {
                Enemies.Add(i);
            }
        }
        foreach (Transform i in fov.stereoVisibleTargets)
        {
            if (i.tag == "Rapty")
            {
             Enemies.Add(i);
            }
        }

        friends.Clear();
        foreach (Transform u in fov.visibleTargets)
        {
            if (u.tag == "Anky")
            {
                if (u.transform.position != transform.position)
                {
                    friends.Add(u);
                }
            }
        }
        foreach (Transform u in fov.stereoVisibleTargets)
        {
            if (u.tag == "Anky")
            {
                if (u.transform.position != transform.position)
                {
                    friends.Add(u);
                }
            }
        }

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
        stateMachine.Update();
        base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.tag == "Rapty")
        {
            takeDamage(30);
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
