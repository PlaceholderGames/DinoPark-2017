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

    public double Health = 200;
    public double Food = 100;
    public double Water = 100;
    public Animator anim;
    //public Stats StatScript;
    public FieldOfView fov;
    public ankyState currentstate;
    private Wander wanderScript;
    private Flee FleeScript;
    private Seek SeekScript;
    private Face faceScript;
    private Attack attackScript;
    public GameObject water;

    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        FleeScript = GetComponent<Flee>();
        wanderScript = GetComponent<Wander>();
        SeekScript = GetComponent<Seek>();
        faceScript = GetComponent<Face>();
        attackScript = GetComponent<Attack>();
        SeekScript.target = water;
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", true);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);

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
        currentstate = ankyState.GRAZING;// idle go to grazing
    }
    void grazing()
    {
        if (Health <= 0)
        {
            Health = 0;
            currentstate = ankyState.DEAD;
        }
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        wanderScript.enabled = true;
        SeekScript.enabled = true;

        if (Food <= 40)
        {
            currentstate = ankyState.EATING;

        }
        else if (Water <= 70)
        {
            currentstate = ankyState.DRINKING;
            wanderScript.enabled = true;
        }
        else
        {
            currentstate = ankyState.IDLE;
        }
        foreach (Transform i in fov.visibleTargets)
        {
            float dist2;
            dist2 = Vector3.Distance(this.transform.position, i.transform.position);
            if (i.tag == ("Rapty") && dist2 <= 50)
            {
                currentstate = ankyState.ALERTED;
            }
        }
    }
    void alert()
    {
        if (Health <= 0)
        {
            Health = 0;
            currentstate = ankyState.DEAD;
        }
        foreach (Transform i in fov.visibleTargets)
        {
            float dist2;
            if (i.tag == ("Rapty"))
            {
                dist2 = Vector3.Distance(this.transform.position, i.transform.position);
                //Debug.Log(dist2);

                if (dist2 <= 30)
                {
                    currentstate = ankyState.FLEEING; // grazing goes to alert
                }
                else
                {
                    if (dist2 >= 50)
                    {
                        currentstate = ankyState.GRAZING;
                    }
                }

                foreach (Transform j in fov.visibleTargets)
                {
                    if (j.tag == ("anky"))
                    {
                        dist2 = Vector3.Distance(this.transform.position, i.transform.position);
                        // Debug.Log(dist2);
                        // Debug.Log("i can see my Friend");
                        {
                            if (dist2 <= 50)
                            {
                                faceScript.enabled = true;
                                SeekScript.enabled = true;
                            }
                            else
                            {
                                currentstate = ankyState.GRAZING;
                            }
                        }
                    }
                }

            }
        }
    }
    void eatting()
    {
        if (Health <= 0)
        {
            Health = 0;
            currentstate = ankyState.DEAD;
        }
        if (transform.position.y <= 62)
        {
            Food -= Time.deltaTime;
            // Debug.Log(Food);
        }
        if (Food <= 0)
        {
            Food = 0;
        }
        if (Health <= 0)
        {
            Health = 0;
        }
        if (Food >= 100)
        { 
         currentstate = ankyState.GRAZING;
        }
     }

    void drinking()
    {
        if (Health <= 0)
        {
            Health = 0;
            currentstate = ankyState.DEAD;
        }
        wanderScript.enabled = false;
        SeekScript.enabled = true;
        
        if (transform.position.y <= 36)
        {
            SeekScript.enabled = false;
            Water += (Time.deltaTime *3) * 1.0;
            if(Water <=0)
            {
                Water = 0;
            }


        }
        if(Water >=100)
        {
           
            currentstate = ankyState.GRAZING;
       

        } 

        
    }
    void Fleeing() {
        if (Health <= 0)
        {
            Health = 0;
            currentstate = ankyState.DEAD;
        }
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == ("Rapty"))
            {
                float dist;
                dist = Vector3.Distance(this.transform.position, i.transform.position);
                //Debug.Log(dist);
                //Debug.Log("anky");


                if (dist <= 30)
                {      
                    wanderScript.enabled = false;
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isEating", false);
                    anim.SetBool("isDrinking", false);
                    anim.SetBool("isAlerted", false);
                    anim.SetBool("isGrazing", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isFleeing", true);
                    anim.SetBool("isDead", false);
                    FleeScript.target = i.gameObject;
             
                    FleeScript.enabled = true;
                   Debug.Log("you underestermate my power");

                }
                if (dist >= 40)
                {
                    currentstate = ankyState.ALERTED; // fleeing goes to alerting
                    wanderScript.enabled = true;
                    FleeScript.enabled = false;
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isEating", false);
                    anim.SetBool("isDrinking", false);
                    anim.SetBool("isAlerted", false);
                    anim.SetBool("isGrazing", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isFleeing", true);
                    anim.SetBool("isDead", false);
                    Debug.Log("i am no longer fleeing");
                }

            }
        }

    }
    void dead()
    {
        anim.SetBool("isIdle", false); //alerted to attacking
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", true);
        Debug.Log(" I HATE YOU!");
        DestroyObject(gameObject);
        
    }
    void attack()
    {
        if (Health <= 0)
        {
            Health = 0;
            currentstate = ankyState.DEAD;
        }
        if (Health >=75)
        {
            foreach(Transform i in fov.visibleTargets)
                {
                if( i.tag == ("rapty" ))
                    {
                    float dist3;
                    dist3 = Vector3.Distance(this.transform.position, i.transform.position);
                   // Debug.Log(dist3);
                   // Debug.Log("the Raptor is to close");

                    if (dist3 <=15)
                        {
                        currentstate = ankyState.ATTACKING; //fleeing to attacking 
                        anim.SetBool("isIdle", false); //alerted to attacking
                        anim.SetBool("isEating", false);
                        anim.SetBool("isDrinking", false);
                        anim.SetBool("isAlerted", false);
                        anim.SetBool("isGrazing", false);
                        anim.SetBool("isAttacking", true);
                        anim.SetBool("isFleeing", false);
                        anim.SetBool("isDead", false);
                        transform.Rotate(90, 0, 0);
                        attackScript.enabled = true;

                        }
                    else if (Health <=75)
                    {
                        currentstate = ankyState.FLEEING;
                        
                    }
                   
                
                    }

                }
        }
    }
    protected override void Update()
    {
        Water -= (Time.deltaTime * 0.2) * 1.0;
        Food -= (Time.deltaTime * 0.1) * 1.0;
        if (Food <= 0)
        {
            Food = 0;
        }
        
        if (Food <= 0)
        {
            Health -= 1 * Time.deltaTime;
        }
        else if (Water == 0 )
        {
            Health -= 1 * Time.deltaTime;
        }
        else if (Food == 0 && Water == 0)
        {
            Health -= 3 * Time.deltaTime;
        }

        Debug.Log(currentstate);
        switch (currentstate)
        {
            
            case ankyState.ALERTED:
                alert();
                break;

            case ankyState.ATTACKING:
                attack();
                break;

            case ankyState.DEAD:
                dead();
                break;

            case ankyState.DRINKING:
                drinking();
                break;

            case ankyState.EATING:
                eatting();
                break;

            case ankyState.FLEEING:
                Fleeing();
                break;
            case ankyState.GRAZING:
                grazing();
                break;
            case ankyState.IDLE:
                idle();
                break;
        }
             
             
         

        
        

      
      
        base.Update();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
