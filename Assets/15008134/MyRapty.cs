using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyRapty : Agent
{




    public float speed = 10.0f;
    public AStarSearch aStarScript;
    public ASPathFollower pathFolloweScript;
    public FieldOfView fov;
    public ASPathNode asPathNode;
    public raptyState currentState;
    public raptyState previousState;
    public Flee fleeScript;
    public Wander wanderScript;
    public GameObject waterSphere;
    public GameObject lunch;
    public GameObject friendo;

    public bool isDead = false;

    public float health = 100;
    public float hunger = 100;
    public float thirst = 100;
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
    private Animator anim;

    // Use this for initialization
    protected override void Start()
    {

        aStarScript = GetComponent<AStarSearch>();
        pathFolloweScript = GetComponent<ASPathFollower>();
        asPathNode = GetComponent<ASPathNode>();
        fleeScript = GetComponent<Flee>();
        wanderScript = GetComponent<Wander>();

        anim = GetComponent<Animator>();
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
    }

    protected override void Update()
    {
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed





        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 150, transform.position.z);
        }

        if (Vector3.Distance(this.transform.position, waterSphere.transform.position) > 10)
        {
            thirst -= 2 * Time.deltaTime;
        }

        if (thirst <= 0)
        {
            thirst = 0;
        }

        if (Vector3.Distance(this.transform.position, lunch.transform.position) > 20)
        {
            hunger -= 1 * Time.deltaTime;
        }

        if (hunger <= 0)
        {
            hunger = 0;
        }

        if (hunger == 0 || thirst == 0)
        {
            health -= 1 * Time.deltaTime;
        }
        else if (hunger == 0 && thirst == 0)
        {
            health -= 3 * Time.deltaTime;
        }
        if (health >= 100)
        {
            health = 100;
        }
        // if hunger / thirst == 0
        //over time 
        //health -1 
        //if health == 0
        //destroy

        switch (currentState)
        {
            case raptyState.ALERTED:
                alertedState();
                break;
            case raptyState.ATTACKING:
                attackingState();
                break;
            case raptyState.DEAD:
                deadState();
                break;
            case raptyState.DRINKING:
                drinkingState();
                break;
            case raptyState.EATING:
                eatingState();
                break;
            case raptyState.FLEEING:
                fleeingState();
                break;
            case raptyState.HUNTING:
                huntingState();
                break;
            case raptyState.IDLE:
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
    float dist2 = 0;


    void idleState()
    {
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        if (health <= 0)
        {
            currentState = raptyState.DEAD;
            previousState = raptyState.IDLE;
        }
        //To HUNTING
        wanderScript.enabled = true;
        currentState = raptyState.HUNTING;
        previousState = raptyState.IDLE;
    }

    void huntingState()
    {
        bool alertednow = false;
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        if (health <= 0)
        {
            currentState = raptyState.DEAD;
            previousState = raptyState.HUNTING;
        }
        else
        {
            foreach (Transform i in fov.visibleTargets)
            {
                foreach (Transform j in fov.visibleTargets)
                {
                    dist = Vector3.Distance(this.transform.position, i.position);
                    if (i.gameObject.tag == "Anky" && ((dist >= 1 && dist < 40) && health > 50))
                    {
                        //to attacking
                        Debug.Log("let me eat you");
                        currentState = raptyState.ATTACKING;
                        previousState = raptyState.ALERTED;

                    }
                    else if (i.tag == "Anky" && (dist >= 1 && dist < 40) && health <= 50)
                    {
                        //to attacking
                        Debug.Log("lmao naaaa");
                        currentState = raptyState.FLEEING;
                        previousState = raptyState.ALERTED;

                    }
                    else if (i.tag != "Anky" && j.tag == "Rapty")
                    {
                        if ((Vector3.Distance(this.transform.position, friendo.transform.position) > 20) && (Vector3.Distance(this.transform.position, friendo.transform.position) < 60))
                        {
                            Debug.Log("Oooh Friend");
                            aStarScript.target = this.friendo.gameObject;
                            if (pathFolloweScript.path.nodes.Count < 1 || pathFolloweScript.path == null)
                            {
                                pathFolloweScript.path = aStarScript.path;
                            }
                            move(pathFolloweScript.getDirectionVector());
                        }
                        currentState = raptyState.ALERTED;
                        previousState = raptyState.HUNTING;
                    }
                    
                }
            }
            if (thirst <= 30)
            {
                //to drinking
                Debug.Log("Need Drink");
                currentState = raptyState.DRINKING;
                previousState = raptyState.HUNTING;
            }
            else if (alertednow)
            {
                //to idle
                Debug.Log("Idling");
                currentState = raptyState.IDLE;
                previousState = raptyState.HUNTING;
                wanderScript.enabled = true;
            }
        }
    }

    void alertedState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", true);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        Debug.Log("yes");
        if (health <= 0)
        {
            currentState = raptyState.DEAD;
            previousState = raptyState.ALERTED;
        }
        else
        {
            foreach (Transform i in fov.visibleTargets)
            {
                foreach (Transform j in fov.visibleTargets)
                {
                    dist = Vector3.Distance(this.transform.position, i.position);
                    Debug.Log("Dist to Anky");
                    Debug.Log(dist);
                    dist2 = Vector3.Distance(this.transform.position, j.position);
                    Debug.Log("Dist to Rapty");
                    Debug.Log(dist2);

                    if (i.tag == "Anky" && (dist >= 1 && dist <= 60) && health > 50)
                    {
                        //to attacking
                        Debug.Log("let me eat you");
                        currentState = raptyState.HUNTING;
                        previousState = raptyState.ALERTED;

                    }
                    if (thirst <= 30)
                    {
                        Debug.Log("Need Drink");
                        //to drinking
                        currentState = raptyState.DRINKING;
                        previousState = raptyState.ALERTED;
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
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", true);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        
        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {

                dist = Vector3.Distance(this.transform.position, i.position);
                Debug.Log("Dist to Anky");
                Debug.Log(dist);
                dist2 = Vector3.Distance(this.transform.position, j.position);
                Debug.Log("Dist to Rapty");
                Debug.Log(dist2);
                if (health > 20)
                {
                    aStarScript.target = this.lunch.gameObject;
                    if (pathFolloweScript.path.nodes.Count < 1 || pathFolloweScript.path == null)
                    {
                        pathFolloweScript.path = aStarScript.path;


                    }
                    move(pathFolloweScript.getDirectionVector());

                    if (Vector3.Distance(lunch.transform.position, this.transform.position) < 20)
                    {
                        isDead = lunch.GetComponent<MyAnky>().takeDamage(1);

                        if (isDead)
                        {
                            currentState = raptyState.EATING;
                            previousState = raptyState.ATTACKING;
                        }
                    }
                    
                }

                else if (health <= 20)
                {
                    //to fleeing
                    Debug.Log("I need healing");
                    currentState = raptyState.FLEEING;
                    previousState = raptyState.ATTACKING;

                }
                if (health <= 0)
                {
                    //to dead
                    currentState = raptyState.DEAD;
                    previousState = raptyState.ALERTED;
                }
            }
        }
        if (health <= 0)
        {
            currentState = raptyState.DEAD;
            previousState = raptyState.ATTACKING;
        }
        else
        {
                //to alerted
                Debug.Log("He gone");
                currentState = raptyState.HUNTING;
                previousState = raptyState.ATTACKING;

            
        }
    }
    void deadState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", true);
        anim.SetFloat("speedMod", 1.0f);
        //Dead is dead
        Debug.Log("Insert Coin to respawn...");
        DestroyObject(gameObject);
    }
    void eatingState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", true);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        if (health <= 0)
        {
            currentState = raptyState.DEAD;
            previousState = raptyState.EATING;
        }
        else if (health > 1)
        {
            for (int i = 0; i < 1; i++)
            {
                if (health <= 100)
                {

                    health = health + 50;
                    if (health >= 100)
                    {
                        health = 100;
                    }
                }
                //for loop to add hunger up to 100
                Debug.Log("Om Nom Nom");


                //When collides with Meat Ball
                //Destroy meat Ball
                //Food upped

            }
            if (hunger >= 100)
            {
                if (hunger >= 100)
                    hunger = 100;
                //to HUNTING
                Debug.Log("All full");
                currentState = raptyState.ATTACKING;
                previousState = raptyState.EATING;
            }
        }
        else 
        {
            Debug.Log("Not quite full");
            currentState = raptyState.ATTACKING;
            previousState = raptyState.EATING;
        }
    }
    void drinkingState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", true);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        if (health <= 0)
        {
            currentState = raptyState.DEAD;
            previousState = raptyState.DRINKING;
        }
        //to alerted
        //to HUNTING

        //to drink

        aStarScript.target = this.waterSphere.gameObject;
        if (pathFolloweScript.path.nodes.Count < 1 || pathFolloweScript.path == null)
        {
            pathFolloweScript.path = aStarScript.path;
        }
        move(pathFolloweScript.getDirectionVector());
        if (Vector3.Distance(this.transform.position, waterSphere.transform.position) < 10 && thirst < 100)
        {
            Debug.Log("Sip sip");
            //for loop to add hunger up to 100
            thirst += 5 * Time.deltaTime;


            previousState = raptyState.DRINKING;
            if (thirst >= 100)
                thirst = 100;

        }
        else if (thirst == 100)
        {
            //to HUNTING
            Debug.Log("All liquid full");
            currentState = raptyState.HUNTING;
            previousState = raptyState.DRINKING;
        }
        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {

                dist = Vector3.Distance(this.transform.position, i.position);
                Debug.Log("Dist to Anky");
                Debug.Log(dist);
                dist2 = Vector3.Distance(this.transform.position, j.position);
                Debug.Log("Dist to Rapty");
                Debug.Log(dist2);

                if (i.tag == "Anky" && dist < 60)
                {

                    Debug.Log("mmm i see lunch");
                    //to alert
                    currentState = raptyState.ALERTED;
                    previousState = raptyState.DRINKING;
                }
            }
        }
    }
    void fleeingState()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHUNTING", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", true);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);

        if (health <= 0)
        {
            currentState = raptyState.DEAD;
            previousState = raptyState.FLEEING;
        }
        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {

                dist = Vector3.Distance(this.transform.position, i.position);
                Debug.Log("Dist to Anky");
                Debug.Log(dist);
                dist2 = Vector3.Distance(this.transform.position, j.position);
                Debug.Log("Dist to Rapty");
                Debug.Log(dist2);

                if (i.tag == "Anky")
                {

                    if (dist < 40 && health >= 51)
                    {
                        Debug.Log("Charge!!");
                        currentState = raptyState.ATTACKING;
                        previousState = raptyState.FLEEING;
                    }
                    else if (dist < 40 && health <= 50)
                    {
                        Debug.Log("i give up!");
                        previousState = raptyState.FLEEING;
                        fleeScript.target = i.gameObject;
                        fleeScript.enabled = true;
                        wanderScript.enabled = false;
                    }
                    else if (dist >= 40)
                    {
                        Debug.Log("Stay Away");
                        currentState = raptyState.HUNTING;
                        previousState = raptyState.FLEEING;

                        fleeScript.enabled = false;
                        wanderScript.enabled = true;

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

    public bool takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyObject(gameObject);
            return true;
        }
        return false;
    }
}
