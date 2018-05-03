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

    public List<Transform> RaptorsInView = new List<Transform>();
    public List<Transform> AnkyInView = new List<Transform>();
    
    public FieldOfView fov;
    public Animator anim;
    
    public Flee fleeScript;
    public Wander wanderScript;
    public Seek seekingScript;
    public TScript TerrainScript;
    
    public GameObject Water;//Daylight Water placed in the centre of a pool of water
    public GameObject littleAnky;
    
    public ankyState currentAnkyState;

    public double hydration = 100; //Thirst
    public double sustenance = 100; //Hunger
    public double health = 100; //Health

    public int damage = 2;
    public int numberOfChildren = 0;

    private float waitTime = 2;
    private float timer;
    private float breedingWaitTime = 15;
    private float breedingTimer;

    public bool hasChild = false;

    public StateMachine<MyAnky> stateMachine { get; set; }

    void Awake()
    {
        fleeScript = GetComponent<Flee>();
        wanderScript = GetComponent<Wander>();
        seekingScript = GetComponent<Seek>();
        
    }

    // Use this for initialization
    protected override void Start()
    {

        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(GrazingState.Instance);
        anim = GetComponent<Animator>();
        
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller

        seekingScript.target = Water;
        seekingScript.enabled = false;//Disabling all scripts that are not wandering on start
        fleeScript.enabled = false;
        base.Start();

    }

    protected override void Update()
    {
        
        if(currentAnkyState != ankyState.DEAD)
        {
            Search();
            desireToLive();
            waiting();
            if(hasChild==true)
            {
                breedingWaiting();
            }
            
        } 
       if(health <= 0 && currentAnkyState != ankyState.DEAD)
        {
            stateMachine.ChangeState(DeadState.Instance);
            currentAnkyState = ankyState.DEAD;
        }

        stateMachine.Update();

    }


    public bool takeDamage(int damage)
    {
        health -= damage;
        if(health <=0)
        {
            //Debug.Log("Dead");
            return true;
        }
        return false;
    }


    /*
    public Vector3 rotationDirection;
    public float durationTime;
    public float smoothTime;
    private float convertedTime = 200;
    private float smooth;
    public void Attack()
    {
        smooth = Time.deltaTime * smoothTime * convertedTime;
        transform.Rotate(rotationDirection * smooth);
    }
    */
    protected void Search()
    {
        RaptorsInView.Clear();
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Rapty" && !RaptorsInView.Contains(i))
            {
                RaptorsInView.Add(i);
            }
        }

        foreach (Transform i in fov.stereoVisibleTargets)
        {
            if (i.tag == "Rapty" && !RaptorsInView.Contains(i))
            {
                RaptorsInView.Add(i);
            }
        }

        AnkyInView.Clear();
        
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Anky" && !AnkyInView.Contains(i))
            {
                if(i.transform.position != transform.position)
                    AnkyInView.Add(i);
            }
        }

        foreach (Transform i in fov.stereoVisibleTargets)
        {
            if (i.tag == "Anky" && !AnkyInView.Contains(i))
            {
                if (i.transform.position != transform.position)
                    AnkyInView.Add(i);
            }
        }
    }

    protected void desireToLive()
    {
        if(hydration > 0)
            hydration -= (Time.deltaTime * 0.2);

        if(sustenance > 0)
            sustenance -= (Time.deltaTime * 0.25);

        if(hydration <=0 || sustenance <=0)
        {
            health -= 0.1;
        }

        if(hydration >85 && sustenance > 85 && health <100)
        {
            health += 0.2;
        }

    }

    public void breeding()
    {
        foreach (Transform i in AnkyInView)
        {
            float distance = Vector3.Distance(i.transform.position, transform.position);
            if(distance < 10)
            {
                Vector3 spawnPos = new Vector3(transform.position.x + 4, transform.position.y + 2, transform.position.z + 4);
                littleAnky.transform.position = spawnPos;
                if (timer > waitTime)
                {
                    Instantiate(littleAnky);
                    hasChild = true;
                    numberOfChildren += 1;
                    timer = 0.0f;
                }
                
            }
        }
    }

    protected void waiting()
    {
        timer += Time.deltaTime;     
    }

    protected void breedingWaiting()
    {
        breedingTimer += Time.deltaTime;
        if(breedingTimer >= breedingWaitTime)
        {
            hasChild = false;
            breedingTimer = 0.0f;
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

}