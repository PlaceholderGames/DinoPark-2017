using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyRapty : Agent
{

    private Flee fleeScript;
    private Wander wanderScript;

    public AStarSearch aStarScript;
    public ASPathFollower pathFollowerScript;

    public GameObject waterSphere;
    public GameObject huntingSphere;
    public FieldOfView fov;
    public raptyState prevState;
    public raptyState currentState;

    public List<GameObject> ankyList = new List<GameObject>();
    public List<GameObject> deadankyList = new List<GameObject>();
    public List<GameObject> raptyList = new List<GameObject>();

    float speed = 15.0f;
    float timer = 100;
    bool once = false;
    public double water = 100;
    public double food = 100;
    public double health = 100;

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
        anim = GetComponent<Animator>();
        fleeScript = GetComponent<Flee>();
        wanderScript = GetComponent<Wander>();
        aStarScript = GetComponent<AStarSearch>();
        pathFollowerScript = GetComponent<ASPathFollower>();
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        currentState = raptyState.IDLE;
        wanderScript.enabled = true;
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
    }

    protected override void Update()
    {
        water -= 0.1 * Time.deltaTime;
        food -= 2 * Time.deltaTime;

        if (health <= 0)
        {
            currentState = raptyState.DEAD;
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
            health += 3 * Time.deltaTime;
            food -= 5 * Time.deltaTime;
        }

        switch (currentState)
        {
            case raptyState.IDLE:
                idleMethod();
                break;
            case raptyState.EATING:
                eatingMethod();
                break;
            case raptyState.DRINKING:
                drinkingMethod();
                break;
            case raptyState.ALERTED:
                alertMethod();
                break;
            case raptyState.HUNTING:
                huntingMethod();
                break;
            case raptyState.FLEEING:
                fleeingMethod();
                break;
            case raptyState.ATTACKING:
                combat();
                break;
            case raptyState.DEAD:
                death();
                break;
            default:
                break;
        }
        base.Update();
    }
    void idleMethod()
    {
        Debug.Log("Idle");
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        ankyList.Clear();

        raptyList.Add(this.gameObject);
        currentState = raptyState.ALERTED;
        prevState = raptyState.IDLE;

    }
    void eatingMethod()
    {
        // deadankyList.Clear();
        if (fov.visibleTargets != null)
        {
            foreach (Transform i in fov.visibleTargets)     //see
            {
                if (i.tag == "Anky")                    // anky
                {
                    // deadankyList.Add(i.gameObject)
                    if (i.GetComponent<MyAnky>().currentState == MyAnky.ankyState.DEAD)     //anky dead
                    {
                        i.GetComponent<MyAnky>().isDead = true;
                        Vector3.MoveTowards(this.gameObject.transform.position, i.gameObject.transform.position, 10);
                        if (Vector3.Distance(this.gameObject.transform.position, i.gameObject.transform.position) <= 5)
                        {
                            food += 10 * Time.deltaTime;
                        }
                    }
                }
            }
        }
        currentState = raptyState.ALERTED;
    }

    void drinkingMethod()
    {
        float waterDist = Vector3.Distance(this.transform.position, waterSphere.transform.position);
        if (this.transform.position.y <= 36 && water < 100)
        {

            anim.SetBool("isIdle", true);
            anim.SetBool("isEating", false);
            anim.SetBool("isDrinking", false);
            anim.SetBool("isAlerted", false);
            anim.SetBool("isHunting", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isFleeing", false);
            anim.SetBool("isDead", false);
            water += 4 * Time.deltaTime;
        }
        else if (water >= 100)
        {
            water = 100;
            currentState = raptyState.ALERTED;
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
        prevState = raptyState.DRINKING;
    }
    void fleeingMethod()
    {
        fleeScript.enabled = true;
        timer--;
        if (timer <= 0)
        {
            currentState = raptyState.ALERTED;
            timer = 100;
            fleeScript.enabled = false;
        }
        
    }

    void alertMethod()
    {
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        wanderScript.enabled = true;
        if (fov.visibleTargets != null)
        {
            foreach (Transform i in fov.visibleTargets)
            {
                if (i.tag == "Rapty")
                {
                    float distRR;
                    distRR = Vector3.Distance(this.transform.position, i.transform.position);
                    if (distRR > 150)
                    {
                        //aStarScript.target = this.gameObject;
                        //this.i.transform.position = aStarScript.mapGrid.getGrassLocation(this.gameObject).position;
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
        
        if (food < 50)
        {
            currentState = raptyState.HUNTING;
        }
        else if (water < 20)
        {
            currentState = raptyState.DRINKING;
        }
    }

    void huntingMethod()
    {
        ankyList.Clear();
        if (fov.visibleTargets != null)
        {
            foreach (Transform i in fov.visibleTargets)
            {
                if (i.tag == "Anky")
                {
                    ankyList.Add(i.gameObject);
                }
            }
        }
        if (water <= 20)
        {
            currentState = raptyState.DRINKING;
        }

        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);

        if (Vector3.Distance(this.gameObject.transform.position, this.huntingSphere.transform.position) < 10)
        {

            once = false;
            this.huntingSphere.GetComponent<moveLocationRandom>().active = once;
        }
        if (once == false)
        {
            once = true;
            this.huntingSphere.GetComponent<moveLocationRandom>().active = once;
            this.huntingSphere.GetComponent<moveLocationRandom>().moveSphere();
        }
        if (ankyList.Count <= 0)
        {
            aStarScript.target = this.gameObject;
            //this.huntingSphere.transform.position = aStarScript.mapGrid.findAnky().position; 

            aStarScript.target = this.huntingSphere.gameObject;
            if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
            {
                pathFollowerScript.path = aStarScript.path;
            }
            move(pathFollowerScript.getDirectionVector());

        }
        else if (ankyList.Count > 0)
        {
            float distRA = Vector3.Distance(this.gameObject.transform.position, ankyList[0].transform.position);
            aStarScript.target = this.gameObject;
            //this.huntingSphere.transform.position = aStarScript.mapGrid.findAnky(this.gameObject).position;
            aStarScript.target = ankyList[0].gameObject;
            if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
            {
                pathFollowerScript.path = aStarScript.path;
            }
            move(pathFollowerScript.getDirectionVector());
            if (distRA < 10)
            {
                currentState = raptyState.ATTACKING;
            }
        }
    }

    void combat()
    {
        bool hit = false;
        ankyList.Clear();
        if (fov.visibleTargets != null)
            foreach (Transform i in fov.visibleTargets)
            {
                if (i.tag == "Anky")
                {
                    ankyList.Add(i.gameObject);
                    aStarScript.target = ankyList[0].gameObject;
                    float distRA = Vector3.Distance(this.gameObject.transform.position, ankyList[0].transform.position);
                    if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
                    {
                        pathFollowerScript.path = aStarScript.path;
                    }
                    move(pathFollowerScript.getDirectionVector());
                    if (distRA < 5)
                    {
                        hit = raptyList[0].GetComponent<MyRapty>().takeDamage(5);
                        if (hit)
                        {
                            currentState = raptyState.ALERTED;
                        }
                    }

                }
            }
    }
    
    void death()
    {
        Debug.Log("Death");

        DestroyObject(this.gameObject);
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
            currentState = raptyState.DEAD;
            return true;
        }
        return false;
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
