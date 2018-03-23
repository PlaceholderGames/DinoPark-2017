using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyRapty : Agent
{
    public enum raptyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on location of a target object (killed prey)
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        HUNTING,    // Moving with the intent to hunt
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };

    public double Health = 100;
    public double Food = 150;
    public double Water = 100;
    float dist;
    private Animator anim;
    private Wander wanderScript;
    private Flee FleeScript;
    private Seek SeekScript;

    private Pursue PursueScript;
    private Attack attackScript;
    private Face FaceScript;

    public FieldOfView fov;
    public raptyState currentstate;
   // public raptyState prevstate;
    public GameObject water;

    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        FleeScript = GetComponent<Flee>();
        wanderScript = GetComponent<Wander>();
        SeekScript = GetComponent<Seek>();
        FaceScript = GetComponent<Face>();
        attackScript = GetComponent<Attack>();
        PursueScript = GetComponent<Pursue>();
        SeekScript.target = water;
      

        SeekScript.target = water;
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

    }

    protected override void Update()
    {

        Water -= (Time.deltaTime * 0.2) * 1.0;
        Food -= (Time.deltaTime * 0.1) * 1.0;
        Debug.Log(currentstate);
        switch (currentstate)
        {

            case raptyState.IDLE:
                idle();
                break;

            case raptyState.HUNTING:
                Hunting();
                break;

            case raptyState.ALERTED:
                alert();
                break;

            case raptyState.ATTACKING:
                attack();
                break;

            case raptyState.EATING:
                eatting();
                break;

            case raptyState.FLEEING:
                  Fleeing();
                break;
            case raptyState.DEAD:
                dead();
                break;
            case raptyState.DRINKING:
                drinking();
                break;
        }

        base.Update();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    void idle()
    {
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        currentstate = raptyState.HUNTING;// idle go to Hunting
    
    }
    void Hunting()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        wanderScript.enabled = true;

    
        foreach (Transform a in fov.visibleTargets)
        {
            float dist2;
            dist2 = Vector3.Distance(this.transform.position, a.transform.position);
            if (a.tag == ("Anky") && dist2 <= 50)
            {
                currentstate = raptyState.ALERTED;
            }
        
        else
        {
                currentstate = raptyState.HUNTING;
        }
       } 
    }
    void alert()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", true);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        foreach (Transform a in fov.visibleTargets)
        {
           
            if(a.tag ==("Anky"))
            {
            float dist2;
            dist2 = Vector3.Distance(transform.position, a.transform.position);
                if (dist2 <= 30) 
                {
                    Debug.Log("Rapty:its over anky i have the high ground");
                    currentstate = raptyState.ATTACKING;
                    wanderScript.enabled = true;
                }
                else
                {
                    if (dist2 >=50)
                    {
                        currentstate = raptyState.HUNTING;
                    }
                }
            }

        }
    }
    void eatting()
    {
        if (transform.position.y <= 62)
        {
            Food -= Time.deltaTime;
            // Debug.Log(Food);
        }
        else if (Food < 100)
        {
            Food += 5;
            if (Food == 100)
            {
                // Debug.Log("Food is full");
                currentstate = raptyState.HUNTING; // grazing to eatting 


            }
            if (Food == 0)
            {

                Health -= 1;
            }
        }
    }
    void drinking()
    {
    
        SeekScript.enabled = true;

        if (transform.position.y <= 36)
        {
            SeekScript.enabled = false;
            Water += (Time.deltaTime * 3) * 1.0;

        }
        if (Water >= 100)
        {

            currentstate = raptyState.HUNTING;


        }


    }
    void Fleeing()
    { }

    void dead()
    {
        if (Health <= 0)
        {
            transform.Rotate(0, 0, 180);
        }
    }
    void attack()
    {


        foreach (Transform a in fov.visibleTargets)
        {
            if (a.tag == ("Anky"))
            {
                float dist3;
                dist3 = Vector3.Distance(this.transform.position, a.transform.position);
                // Debug.Log(dist3);
                if (dist3 <= 30) Debug.Log("dont try it");
                {
                    anim.SetBool("isIdle", false); //alerted to attacking
                    anim.SetBool("isEating", false);
                    anim.SetBool("isDrinking", false);
                    anim.SetBool("isAlerted", false);
                    anim.SetBool("isGrazing", false);
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isFleeing", false);
                    anim.SetBool("isDead", false);
                    wanderScript.enabled = false;
                    SeekScript.enabled = true;
                    FaceScript.enabled = true;
                    SeekScript.target = a.gameObject;
                }
                if(dist3 >=35)
                {
                    currentstate = raptyState.ALERTED;
                    wanderScript.enabled = true;
                    PursueScript.enabled = true;
                    anim.SetBool("isIdle", false); //alerted to attacking
                    anim.SetBool("isEating", false);
                    anim.SetBool("isDrinking", false);
                    anim.SetBool("isAlerted", false);
                    anim.SetBool("isGrazing", false);
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isFleeing", false);
                    anim.SetBool("isDead", false);
                }
            }
        }

    }


}
