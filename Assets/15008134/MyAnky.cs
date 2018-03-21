using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAnky : Agent
{
    public float speed = 10.0f;
    private AStarSearch aStarScript;
    private ASPathFollower pathFolloweScript;
    public FieldOfView fov;
    public ankyState currentState;
    public ankyState previousState;
    public Flee fleeScript;
    public Wander wanderScript;
    //public Pursue pursueScript;
    int health = 100;
    int hunger = 100;
    int thirst = 5;





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

        aStarScript = GetComponent<AStarSearch>();
        pathFolloweScript = GetComponent<ASPathFollower>();
        fleeScript = GetComponent<Flee>();
        wanderScript = GetComponent<Wander>();
        //pursueScript = GetComponent<Pursue>();


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

        // if hunger / thirst == 0
        //over time 
        //health -1 
        //if health == 0
        //destroy

        switch (currentState)
        {
            case ankyState.ALERTED:
                alertedState();
                break;
            case ankyState.ATTACKING:
                attackingState();
                break;
            case ankyState.DEAD:
                deadState();
                break;
            case ankyState.DRINKING:
                drinkingState();
                break;
            case ankyState.EATING:
                eatingState();
                break;
            case ankyState.FLEEING:
                fleeingState();
                break;
            case ankyState.GRAZING:
                grazingState();
                break;
            case ankyState.IDLE:
                idleState();
                break;
            default:
                break;
                Debug.Log("State Error!");
        }



        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }

    float dist = 0;

    
    void idleState()
    {
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);

        //To grazing
        currentState = ankyState.GRAZING;
        previousState = ankyState.IDLE;
        wanderScript.enabled = true;


    }

    void grazingState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {
                if (i.tag == "Rapty" && dist <= 40)
                {
                    //to alerted
                    currentState = ankyState.ALERTED;
                    previousState = ankyState.GRAZING;
                }
                else if (i.tag != "Rapty" || (j.tag != "Anky" || j.tag == "Anky"))
                {

                }
            }
        }
        if (hunger <= 10 && hunger <= thirst)
        {
            //to eating
            currentState = ankyState.EATING;
            previousState = ankyState.GRAZING;
        }
        else if (thirst <= 10 && thirst < hunger)
        {
            //to drinking
            Debug.Log("hello");
            currentState = ankyState.DRINKING;
            previousState = ankyState.GRAZING;
        }
        else
        {
            //to idle
            currentState = ankyState.IDLE;
            previousState = ankyState.GRAZING;
            wanderScript.enabled = true;
        }
       
    }


    void alertedState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", true);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);


        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {
                if (i.tag == "Rapty" && dist <= 10  && health > 50)
                {
                    //to attacking
                    currentState = ankyState.ATTACKING;
                    previousState = ankyState.ALERTED;

                }
                else if (i.tag == "Rapty" && dist <= 10 && health <= 50)
                {
                    //to fleeing
                    currentState = ankyState.FLEEING;
                    previousState = ankyState.ALERTED;
                }
                else if (i.tag == "Rapty" && dist > 31)
                {

                    if (hunger < 10 && hunger <= thirst)
                    {
                        //to eating
                        currentState = ankyState.EATING;
                        previousState = ankyState.ALERTED;
                    }
                    else if (thirst < 10 && thirst < hunger)
                    {
                        //to drinking
                        currentState = ankyState.DRINKING;
                        previousState = ankyState.ALERTED;
                    }
                    else if (i.tag != "Rapty" && (j.tag != "Anky" || j.tag == "Anky"))
                    {
                        //to grazing
                        currentState = ankyState.GRAZING;
                        previousState = ankyState.ALERTED;
                    }

                }
            }
        }
    }
    void attackingState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", true);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);


        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {
                if (health <= 50)
                {
                    //to fleeing
                    currentState = ankyState.FLEEING;
                    previousState = ankyState.ATTACKING;

                }
                else if (i.tag != "Rapty")
                {
                    //to alerted
                    currentState = ankyState.ALERTED;
                    previousState = ankyState.ATTACKING;
                }
                else if (health == 0)
                {
                    //to dead
                    currentState = ankyState.DEAD;
                    previousState = ankyState.ALERTED;
                }
            }
        }
    }
    void deadState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", true);
        anim.SetFloat("speedMod", 1.0f);
        //Dead is dead
        DestroyObject(gameObject);
    }
    void eatingState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", true);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);


        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {
                if (i.tag != "Rapty" && (j.tag != "Anky" || j.tag == "Anky"))
                {
                    if (hunger <= 10)
                    {
                       //eat
                        previousState = ankyState.EATING;

                    }
                    else if (hunger == 100)
                    {
                        //to grazing
                        currentState = ankyState.GRAZING;
                        previousState = ankyState.EATING;
                    }
                }
                else if (i.tag == "Rapty" && dist < 60)
                {
                    //to alert
                    currentState = ankyState.ALERTED;
                    previousState = ankyState.EATING;
                }
            }
        }
    }
    void drinkingState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", true);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        //to alerted
        //to grazing
        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {
                
                if (i.tag == "Rapty" && dist < 60)
                {
                    //to alert
                    currentState = ankyState.ALERTED;
                    previousState = ankyState.DRINKING;
                }
            }
        }
        if (thirst <= 10)
        {
            //to drink
            if (pathFolloweScript.path.nodes.Count < 1 || pathFolloweScript.path == null)
            { 
                pathFolloweScript.path = aStarScript.path;

            move(pathFolloweScript.getDirectionVector());
        }
            //for loop to add hunger up to 100
            previousState = ankyState.DRINKING;

        }
        else if (thirst == 100)
        {
            //to grazing
            currentState = ankyState.GRAZING;
            previousState = ankyState.DRINKING;
        }
    }
    void fleeingState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", true);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);




        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {

                dist = Vector3.Distance(this.transform.position, i.position);
                Debug.Log(dist);
                if (i.tag == "Rapty")
                {
                    if (dist < 10 && health >= 51)
                    {
                        currentState = ankyState.ATTACKING;
                        previousState = ankyState.FLEEING;
                    }
                    else if (dist <= 30)
                    {

                        currentState = ankyState.FLEEING;
                        fleeScript.target = i.gameObject;
                        fleeScript.enabled = true;
                        wanderScript.enabled = false;
                    }
                    else if (dist >= 31 && dist < 60)
                    {
                        currentState = ankyState.ALERTED;
                        previousState = ankyState.FLEEING;

                        fleeScript.enabled = false;
                        wanderScript.enabled = true;

                    }
                    else if (health == 0)
                    {
                        currentState = ankyState.DEAD;
                        previousState = ankyState.FLEEING;
                    }
                }
            }
        }
    }


    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    void move(Vector3 directionVector)
    {
        directionVector *= speed * Time.deltaTime;

        transform.Translate(directionVector, Space.World);
        transform.LookAt(transform.position + directionVector);
    }
}
