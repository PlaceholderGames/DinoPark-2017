using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class MyAnky : Agent
{


    public float speed = 10.0f;
    public AStarSearch aStarScript;
    public ASPathFollower pathFolloweScript;
    public FieldOfView fov;
    public ASPathNode asPathNode;
    public ankyState currentState;
    public ankyState previousState;
    public Flee fleeScript;
    public Wander wanderScript;
    public GameObject waterSphere;
    public GameObject grassSphere;
    public GameObject friendo;
    public GameObject meatSphere;


   
    public  float health = 100;
    public  float hunger = 9;
    public  float thirst = 100;


    


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
        asPathNode = GetComponent<ASPathNode>();
        fleeScript = GetComponent<Flee>();
        
        wanderScript = GetComponent<Wander>();

        //pursueScript = GetComponent<Pursue>();

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
        base.Start();

    }


    protected override void Update()
    {
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
        
        if (Vector3.Distance(this.transform.position, grassSphere.transform.position) > 10)
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
    float dist2 = 0;
   
    
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
        if (health <= 0)
        {
            currentState = ankyState.DEAD;
            previousState = ankyState.IDLE;
        }
        //To grazing
        currentState = ankyState.GRAZING;
        previousState = ankyState.IDLE;
        wanderScript.enabled = true;


    }

    void grazingState()
    {
        bool alertednow = false;
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        if (health <= 0)
        {
            currentState = ankyState.DEAD;
            previousState = ankyState.GRAZING;
        }
        else
        {
            foreach (Transform i in fov.visibleTargets)
            {
                foreach (Transform j in fov.visibleTargets)
                {
                    dist = Vector3.Distance(this.transform.position, i.position);
                    Debug.Log("Dist to Rapty");
                    Debug.Log(dist);
                    dist2 = Vector3.Distance(this.transform.position, j.position);
                    Debug.Log("Dist to Anky");
                    Debug.Log(dist2);

                    if (i.tag == "Rapty" && dist <= 40)
                    {
                        Debug.Log("I am spooped");
                        alertednow = true;
                        //to alerted
                        currentState = ankyState.ALERTED;
                        previousState = ankyState.GRAZING;
                    }
                    else if (i.tag != "Rapty" && j.tag == "Anky")
                    {
                        if ((Vector3.Distance(this.transform.position, friendo.transform.position)> 20) && (Vector3.Distance(this.transform.position, friendo.transform.position) < 60))
                        {
                            Debug.Log("Oooh Friend");
                            aStarScript.target = this.friendo.gameObject;
                            if (pathFolloweScript.path.nodes.Count < 1 || pathFolloweScript.path == null)
                            {
                                pathFolloweScript.path = aStarScript.path;


                            }
                            move(pathFolloweScript.getDirectionVector());
                        }
                    }
                }
            }
            if (hunger <= 30 && hunger <= thirst)
            {
                Debug.Log("Need Noms");
                //to eating
                currentState = ankyState.EATING;
                previousState = ankyState.GRAZING;
            }
            else if (thirst <= 30 && thirst < hunger)
            {
                //to drinking
                Debug.Log("Need Drink");
                currentState = ankyState.DRINKING;
                previousState = ankyState.GRAZING;
            }
            else if (!alertednow)
            {
                //to idle
                Debug.Log("Idling");
                currentState = ankyState.IDLE;
                previousState = ankyState.GRAZING;
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
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        Debug.Log("yes");
        if (health <= 0)
        {
            currentState = ankyState.DEAD;
            previousState = ankyState.ALERTED;
        }
        else {
            foreach (Transform i in fov.visibleTargets)
            {
                foreach (Transform j in fov.visibleTargets)
                {
                    dist = Vector3.Distance(this.transform.position, i.position);
                    Debug.Log("Dist to Rapty");
                    Debug.Log(dist);
                    dist2 = Vector3.Distance(this.transform.position, j.position);
                    Debug.Log("Dist to Anky");
                    Debug.Log(dist2);

                    if (i.tag == "Rapty" && dist <= 10 && health > 50)
                    {
                        //to attacking
                        Debug.Log("For Friendo");
                        currentState = ankyState.ATTACKING;
                        previousState = ankyState.ALERTED;

                    }
                    else if (i.tag == "Rapty" && dist <= 10 && health <= 50)
                    {
                        //to fleeing
                        Debug.Log("Aaaaah Don't Eat Me!");
                        currentState = ankyState.FLEEING;
                        previousState = ankyState.ALERTED;
                    }
                    else if (i.tag == "Rapty" && dist > 31)
                    {

                        if (hunger <= 30 && hunger <= thirst)
                        {
                            //to eating
                            Debug.Log("Need Noms");
                            currentState = ankyState.EATING;
                            previousState = ankyState.ALERTED;
                        }
                        else if (thirst <= 30 && thirst < hunger)
                        {
                            Debug.Log("Need Drink");
                            //to drinking
                            currentState = ankyState.DRINKING;
                            previousState = ankyState.ALERTED;
                        }
                        else if (i.tag != "Rapty" && (j.tag != "Anky" || j.tag == "Anky"))
                        {
                            Debug.Log("Looks Clear");
                            //to grazing
                            currentState = ankyState.GRAZING;
                            previousState = ankyState.ALERTED;
                        }

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
        if (health <= 0)
        {
            currentState = ankyState.DEAD;
            previousState = ankyState.ATTACKING;
        }
        else
        {
            foreach (Transform i in fov.visibleTargets)
            {
                foreach (Transform j in fov.visibleTargets)
                {

                    dist = Vector3.Distance(this.transform.position, i.position);
                    Debug.Log("Dist to Rapty");
                    Debug.Log(dist);
                    dist2 = Vector3.Distance(this.transform.position, j.position);
                    Debug.Log("Dist to Anky");
                    Debug.Log(dist2);
                    if (dist > 15 || health <= 50)
                    {
                        currentState = ankyState.FLEEING;
                        previousState = ankyState.ATTACKING;
                    }
                    else if (i.tag != "Rapty")
                    {
                        //to alerted
                        Debug.Log("Lol He Ded");
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
        Debug.Log("Insert Coin to respawn...");
        DestroyObject(gameObject);
        //When Anky Dead
        //Delete Anky
        //Spawn Meat Ball
        //When collide with Rapty
        //Destroy Meat Ball
        
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
        if (health <= 0)
        {
            currentState = ankyState.DEAD;
            previousState = ankyState.EATING;
        }
        else
        {
            aStarScript.target = this.grassSphere.gameObject;
            if (pathFolloweScript.path.nodes.Count < 1 || pathFolloweScript.path == null)
            {
                pathFolloweScript.path = aStarScript.path;


            }
            move(pathFolloweScript.getDirectionVector());
            if (Vector3.Distance(this.transform.position, grassSphere.transform.position) < 10 && hunger < 100)
            {
                //for loop to add hunger up to 100
                Debug.Log("Om Nom Nom");
                hunger += 5 * Time.deltaTime;



            }
            else if (hunger >= 100)
            {
                if (hunger >= 100)
                    hunger = 100;
                //to grazing
                Debug.Log("All full");
                currentState = ankyState.GRAZING;
                previousState = ankyState.EATING;
            }
            foreach (Transform i in fov.visibleTargets)
            {
                foreach (Transform j in fov.visibleTargets)
                {
                    dist = Vector3.Distance(this.transform.position, i.position);
                    Debug.Log("Dist to Rapty");
                    Debug.Log(dist);
                    dist2 = Vector3.Distance(this.transform.position, j.position);
                    Debug.Log("Dist to Anky");
                    Debug.Log(dist2);

                    if (i.tag == "Rapty" && dist < 60)
                    {
                        //to alert
                        Debug.Log("I am spooped");
                        currentState = ankyState.ALERTED;
                        previousState = ankyState.EATING;
                    }
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
        if (health <= 0)
        {
            currentState = ankyState.DEAD;
            previousState = ankyState.DRINKING;
        }
        //to alerted
        //to grazing

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
            

            previousState = ankyState.DRINKING;
            if (thirst >= 100)
                thirst = 100;
            
        }
        else if (thirst == 100)
        {
            //to grazing
            Debug.Log("All liquid full");
            currentState = ankyState.GRAZING;
            previousState = ankyState.DRINKING;
        }
        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {

                dist = Vector3.Distance(this.transform.position, i.position);
                Debug.Log("Dist to Rapty");
                Debug.Log(dist);
                dist2 = Vector3.Distance(this.transform.position, j.position);
                Debug.Log("Dist to Anky");
                Debug.Log(dist2);

                if (i.tag == "Rapty" && dist < 60)
                {

                    Debug.Log("I am spooped");
                    //to alert
                    currentState = ankyState.ALERTED;
                    previousState = ankyState.DRINKING;
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
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", true);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);

        if (health <= 0)
        {
            currentState = ankyState.DEAD;
            previousState = ankyState.FLEEING;
        }
        foreach (Transform i in fov.visibleTargets)
        {
            foreach (Transform j in fov.visibleTargets)
            {

                dist = Vector3.Distance(this.transform.position, i.position);
                Debug.Log("Dist to Rapty");
                Debug.Log(dist);
                dist2 = Vector3.Distance(this.transform.position, j.position);
                Debug.Log("Dist to Anky");
                Debug.Log(dist2);

                if (i.tag == "Rapty")
                {
                    if (dist < 10 && health >= 51)
                    {
                        Debug.Log("Charge!!");
                        currentState = ankyState.ATTACKING;
                        previousState = ankyState.FLEEING;
                    }
                    else if (dist <= 30)
                    {
                        Debug.Log("Aah Don't eat me!");
                        currentState = ankyState.FLEEING;
                        fleeScript.target = i.gameObject;
                        fleeScript.enabled = true;
                        wanderScript.enabled = false;
                    }
                    else if (dist >= 31 && dist < 60)
                    {
                        Debug.Log("Stay Away");
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
