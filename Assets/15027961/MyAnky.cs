using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class MyAnky : Agent
{

    private Flee fleeScript;
    private Wander wanderScript;

    public AStarSearch aStarScript;
    public ASPathFollower pathFollowerScript;

    public GameObject waterSphere;
    public GameObject grassSphere;
    public FieldOfView fov;
    public ankyState prevState;
    public ankyState currentState;

    public bool isDead = false;

    public List<GameObject> ankyList = new List<GameObject>();
    //public List<GameObject> deadAnkyList = new List<GameObject>();
    public List<GameObject> raptyList = new List<GameObject>();

    float speed = 10.0f;
    float timer = 100;
    public double water = 100;
    public double food = 100;
    public double health = 100;
    public double foodHealth = 100;

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
        fleeScript = GetComponent<Flee>();
        wanderScript = GetComponent<Wander>();
        aStarScript = GetComponent<AStarSearch>();
        pathFollowerScript = GetComponent<ASPathFollower>();
        // Assert default animation booleans and floats
        currentState = ankyState.IDLE;
        wanderScript.enabled = true;
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();

    }

    protected override void Update()
    {

        //Debug.Log(currentState);
        water -= 0.5 * Time.deltaTime;
        food -= 1 * Time.deltaTime;
        //Debug.Log("water");
        //Debug.Log(water);
        //Debug.Log("food");
        //Debug.Log(food <= 30);

        //GetComponent<AStarSearch>().target = waterSphere;
        if (health <= 0)
        {
            currentState = ankyState.DEAD;
        }
        else if (food <= 0 && water <= 0)
        {
            food = 0;
            water = 0;
            health -= 2 * Time.deltaTime;
        }
        else if (water <= 0)
        {
            water = 0;
            health -= 0.01 * Time.deltaTime;
        }
        else if (food <= 0)
        {
            food = 0;
            health -= 0.1 * Time.deltaTime;
        }
        else if (health < 100 && food > 20)
        {
            health += 1 * Time.deltaTime;
            food -= 3 * Time.deltaTime;
        }

        switch (currentState)
        {
            case ankyState.IDLE:
                idleMethod();
                break;
            case ankyState.EATING:
                eatingMethod();
                break;
            case ankyState.DRINKING:
                drinkingMethod();
                break;
            case ankyState.ALERTED:
                alertMethod();
                break;
            case ankyState.GRAZING:
                grazingMethod();
                break;
            case ankyState.FLEEING:
                fleeingMethod();
                break;
            case ankyState.ATTACKING:
                combat();
                break;
            case ankyState.DEAD:
                death();
                break;
            default:
                break;
        }

        // Idle - should only be used at startup
        //  currentState = ankyState.IDLE;
        // Eating - requires a box collision with a dead dino
        //  currentState = ankyState.EATING;
        // Drinking - requires y value to be below 32 (?)
        //  currentState = ankyState.DRINKING;
        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here
        // Fleeing - up to the student what you do here



        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }
    void idleMethod()
    {
        Debug.Log("Idle");
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        ankyList.Clear();

        ankyList.Add(this.gameObject);
        currentState = ankyState.GRAZING;
        prevState = ankyState.IDLE;

    }
    void grazingMethod()
    {
        Debug.Log("Grazing");
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        wanderScript.enabled = true;
        //Debug.Log("Im here!");
        raptyList.Clear();
        ankyList.Clear();
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Anky")
            {
                float distAA;
                distAA = Vector3.Distance(this.transform.position, i.transform.position);
                if (distAA > 100)
                {
                    //aStarScript.target = this.gameObject;
                    //this.grassSphere.transform.position = aStarScript.mapGrid.getGrassLocation(this.gameObject).position;
                    aStarScript.target = i.gameObject;
                    if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
                    {
                        pathFollowerScript.path = aStarScript.path;
                    }
                    move(pathFollowerScript.getDirectionVector());
                }
            }
        }
        if (water <= 30)
        {
            currentState = ankyState.DRINKING;
        }
        else if (food <= 30)
        {
            currentState = ankyState.EATING;
        }
        foreach (Transform i in fov.visibleTargets)
        {
            float distAR;
            distAR = Vector3.Distance(this.transform.position, i.transform.position);
            //Debug.Log(i.tag);
            if (i.gameObject.tag == "Rapty" && distAR < 30)
            {
                raptyList.Add(i.gameObject);
                currentState = ankyState.FLEEING;
            }
            else if (i.gameObject.tag == "Rapty" && distAR > 30)
            {
                raptyList.Add(i.gameObject);
                currentState = ankyState.ALERTED;
            }
        }
        prevState = ankyState.GRAZING;
    }
    void eatingMethod()
    {
        Debug.Log("Eating");
        List<GameObject> raptor = new List<GameObject>();
        raptor.Clear();
        foreach (Transform r in fov.visibleTargets)
        {
            raptor.Add(r.gameObject);
        }

        foreach (GameObject l in raptor)
        {
            if (l.tag == "Rapty" && food > 30)
            {
                currentState = ankyState.ALERTED;
            }
        }

        if (this.transform.position.y >= 70 && food < 100)
        {
            Debug.Log("eating");
            anim.SetBool("isIdle", false);
            anim.SetBool("isEating", true);
            anim.SetBool("isDrinking", false);
            anim.SetBool("isAlerted", false);
            anim.SetBool("isGrazing", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isFleeing", false);
            anim.SetBool("isDead", false); ;
            food += 4 * Time.deltaTime;
        }
        else if (food >= 100)
        {
            Debug.Log("full");
            currentState = ankyState.GRAZING;
        }
        else
        {
            Debug.Log("No food");
            aStarScript.target = this.gameObject;
            this.grassSphere.transform.position = aStarScript.mapGrid.getGrassLocation(this.gameObject).position;
            aStarScript.target = this.grassSphere.gameObject;
            if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
            {
                pathFollowerScript.path = aStarScript.path;
            }
            move(pathFollowerScript.getDirectionVector());
        }
        prevState = ankyState.EATING;
    }
    void drinkingMethod()
    {
        float waterDist = Vector3.Distance(this.transform.position, waterSphere.transform.position);
        if (this.transform.position.y <= 36 && water < 100)
        {

            anim.SetBool("isIdle", false);
            anim.SetBool("isEating", false);
            anim.SetBool("isDrinking", true);
            anim.SetBool("isAlerted", false);
            anim.SetBool("isGrazing", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isFleeing", false);
            anim.SetBool("isDead", false);
            water += 4 * Time.deltaTime;
        }
        else if (water >= 100)
        {
            water = 100;
            currentState = ankyState.GRAZING;
        }
        else
        {
            Debug.Log("Drinking");
            aStarScript.target = this.gameObject;
            this.waterSphere.transform.position = aStarScript.mapGrid.getEdgeWater(this.gameObject).position;
            aStarScript.target = this.waterSphere.gameObject;
            if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
            {
                pathFollowerScript.path = aStarScript.path;
            }
            move(pathFollowerScript.getDirectionVector());
        }
        prevState = ankyState.DRINKING;
    }
    void fleeingMethod()
    {
        Debug.Log("Fleeing");
        foreach (var r in raptyList)
        {
            if (raptyList.Count == 0)
            {
                currentState = ankyState.GRAZING;
            }
            else if (r.tag == "Rapty")
            {
                float distAR;
                distAR = Vector3.Distance(this.transform.position, r.transform.position);
                //Debug.Log(dist);
                if (distAR < 30)
                {
                    currentState = ankyState.FLEEING;
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isEating", false);
                    anim.SetBool("isDrinking", false);
                    anim.SetBool("isAlerted", false);
                    anim.SetBool("isGrazing", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isFleeing", true);
                    anim.SetBool("isDead", false);
                    wanderScript.enabled = false;
                    fleeScript.target = r.gameObject;
                    fleeScript.enabled = true;
                }
                else if (distAR > 40)
                {

                    currentState = ankyState.ALERTED;
                    fleeScript.enabled = false;
                }
            }
        }
        prevState = ankyState.FLEEING;
    }


    void alertMethod()
    {
        raptyList.Clear();
        ankyList.Clear();

        if (fov.visibleTargets.Count <= 0)
        { foreach (Transform i in fov.visibleTargets)
            {

                if (i.gameObject.tag == "Rapty")
                {
                    raptyList.Add(i.gameObject);
                }
            }
        }

        if (fov.visibleTargets.Count <= 0)
        {
            foreach (Transform i in fov.visibleTargets)
            {
                if (i.gameObject.tag == "Anky")
                {
                    ankyList.Add(i.gameObject);
                }
            }
        }
   
        Debug.Log("Alert");
        if (prevState == ankyState.FLEEING)
        {
            timer = 100;
        }
        if (fov.visibleTargets.Count <= 0)
        {
            foreach (Transform i in fov.visibleTargets)
            {

                if (i.tag == "Anky")
                {
                    float distAA;
                    distAA = Vector3.Distance(this.transform.position, i.transform.position);
                    if (distAA > 50)
                    {
                        //aStarScript.target = this.gameObject;
                        //this.grassSphere.transform.position = aStarScript.mapGrid.getGrassLocation(this.gameObject).position;
                        aStarScript.target = i.gameObject;
                        if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
                        {
                            pathFollowerScript.path = aStarScript.path;
                        }
                        move(pathFollowerScript.getDirectionVector());
                    }
                }
            }
        }

        wanderScript.enabled = true;
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", true);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        fleeScript.enabled = false;
        timer--;
        //Debug.Log(timer);
        //if (prevState == ankyState.GRAZING)
        //{

        //}
        if (raptyList.Count >= ankyList.Count)
        {
            currentState = ankyState.ATTACKING;
        }
        if (timer == 0)
        {
            currentState = ankyState.GRAZING;
            timer = 100;
        }
        else if (food <= 10)
        {
            currentState = ankyState.EATING;
        }
        else if (water <= 20)
        {
            currentState = ankyState.DRINKING;
        }
        prevState = ankyState.ALERTED;
    }

    void combat()
    {
        float distAR;
        bool hit = false;

        raptyList.Clear();
        if (fov.visibleTargets.Count <= 0)
        {
            foreach (Transform i in fov.visibleTargets)
            {

                if (i.gameObject.tag == "Rapty")
                {
                    raptyList.Add(i.gameObject);
                }
            }
        }


        if (raptyList.Count >= 1)
        {
            distAR = Vector3.Distance(this.transform.position, raptyList[0].transform.position);

            Debug.Log("Combat");
            aStarScript.target = raptyList[0].gameObject;
            if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
            {
                pathFollowerScript.path = aStarScript.path;
            }
            move(pathFollowerScript.getDirectionVector());
            if (distAR < 5)
            {
                hit = raptyList[0].GetComponent<MyRapty>().takeDamage(10);
                if (hit)
                {
                    currentState = ankyState.ALERTED;
                }
            }
        }
        else
        {
            currentState = ankyState.ALERTED;
        }
    }
    void death()
    {
        Debug.Log("Death");
        wanderScript.enabled = false;
        //deadAnkyList.Add(this.gameObject);
        foodHealth -= 1 * Time.deltaTime;
        if (isDead == true)
        {
            foodHealth -= 10 * Time.deltaTime;
        }
        else if (foodHealth <= 0)
        {
            foodHealth = 0;
            DestroyObject(this.gameObject);
            isDead = false;
        }
    }

    void move(Vector3 directionVector)
    {
        //Debug.Log("I need to drink");
        directionVector *= speed * Time.deltaTime;

        transform.Translate(directionVector, Space.World);
        transform.LookAt(transform.position + directionVector);
    }
    public bool takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currentState = ankyState.DEAD;
            return true;
        }
        return false;
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }


}

