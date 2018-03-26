using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public float avoidDistance = 10.0f;
    public float lookAhead = 100.0f;
    public Transform myT;
    public Animator anim;
    public Collision col;
    public Enemy Raptor;
    public FieldOfView fov;
    public MyFlee fleeing;
    public ankyState currentState;
    public float Health;
    public float dist;
    public Face facing;
    public Pursue pursuing;
    public Collider[] collide;
    public GameObject waterBoy;
    public bool predOverHereBoi;
    public Wander wander;
    public float safeDistance = 20.0f;
    public Agent agent;
    public GameObject target;
    public GameObject pred;
    public Seek seek; 
    public Attack attacking;
    public AStarSearch search;
    public float thirst;
    public float hunger;
    public float thirstinc = 0.0003f;
    // Use this for initialization
    protected override void Start()
    {

        predOverHereBoi = false;
        Health = 100;
        hunger = 100;
        thirst = 100;
        attacking = GetComponent<Attack>();
        StartCoroutine(thirstyboy());
        StartCoroutine(munchies());
       
        anim = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();
        fleeing = GetComponent<MyFlee>();
        pursuing = GetComponent<Pursue>();
        myT = transform;
        wander = GetComponent<Wander>();
        facing = GetComponent<Face>();
        agent = GetComponent<Agent>();
        target = GetComponent<GameObject>();
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
        seenABoy();
        makeChoice();
        isthirsty();
        isHungry();
        
        //mainLoop();
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here



        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }

  


    protected void seenABoy()
    {
   
        foreach(Transform i in fov.visibleTargets)
        {
            if(i.tag == "Rapty")
            {
                pred = i.gameObject;
                predOverHereBoi = true;
            }
         
        }
        foreach(Transform j in fov.stereoVisibleTargets)
        {
           
            if(j.tag == "Rapty")
            {
                pred = j.gameObject;
                predOverHereBoi = true;
            }
           
        }
    }

    IEnumerator thirstyboy()
    {
        while(true)
        {
            yield return new WaitForSeconds(3);
            if(this.thirst > 0)
            {
                this.thirst = this.thirst - 0.5f;
            }

        }
    }

    IEnumerator munchies()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            if (this.hunger > 0)
            {
                this.hunger = this.hunger - 0.5f;
            }

        }
    }

    protected void isHungry()
    {
        if(this.hunger < 50 && !predOverHereBoi)
        {
            this.hunger = this.hunger + 30;
            Debug.Log("Doing me an eat");
        }
    }
    protected void isthirsty()
    {   
        
        if (this.thirst < 50)
        {
            Debug.Log("My y pos = ");
            Debug.Log(this.transform.position.y);
           // Debug.Log("I'm A Thirsty Boy");
            if (fleeing.enabled == false)
            {
                transform.position = Vector3.Lerp(transform.position, waterBoy.transform.position, (2 * Time.deltaTime) / 10);
            }
           if (this.transform.position.y <= 34.4)
           {
               thirst = 100;
               Debug.Log("Drinking");
           }
            
        } 
    }


    protected void makeChoice()
    {
        if (predOverHereBoi)
        {

            dist = Vector3.Distance(transform.position, pred.transform.position);
            Vector3 direction = pred.transform.position - transform.position;
            direction.Normalize();
            Debug.Log(dist);
        }
        else
        {
            dist = 1000;
        }
        if (dist < safeDistance)
        {
            wander.enabled = false;
            fleeing.enabled = false;
            pursuing.enabled = true;
            Debug.Log("Pursuing");
            if(col.gameObject)
            {
                Destroy(this.gameObject);
            }
        }

        else if (dist >= safeDistance && dist <= safeDistance + 100)
        {
            wander.enabled = false;
            fleeing.enabled = true;
            pursuing.enabled = false;
            Debug.Log("Fleeing");
            currentState = ankyState.FLEEING;
        }

        else  if (dist > safeDistance + 100)
        {
            wander.enabled = true;
            fleeing.enabled = false;
            pursuing.enabled = false;
            Debug.Log("wandering");
            currentState = ankyState.IDLE;
        }
    }
    

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}


   