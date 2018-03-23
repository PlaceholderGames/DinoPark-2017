using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateStuff;

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
        DEAD,
        BREEDING
    };

    public Animator anim;
    public StateMachine<MyAnky> stateMachine { get; set; }
    private Flee flee;
    private Wander wander;
    private FieldOfView dinoView;
    private Seek seeking;
    public ankyState currentState = ankyState.IDLE;
    public float health = 100;
    public float water = 100;
    public float food = 100;
    public GameObject Water;
    public GameObject BabyAnky;
    public List<GameObject> raptys;
    public List<GameObject> ankys;
    private GameObject closestAnky;
    private GameObject closestRapty;
    private float ankyDist;
    private float herdDistance = 15.0f;
    public AStarSearch aStar;
    public ASPathFollower asPath;
    public float speed = 10.0f;
    //float timer = 100;


    // Use this for initialization
    protected override void Start()
    {
        flee = GetComponent<Flee>();
        wander = GetComponent<Wander>();
        anim = GetComponent<Animator>();
        dinoView = GetComponent<FieldOfView>();
        seeking = GetComponent<Seek>();
        aStar = GetComponent<AStarSearch>();
        asPath = GetComponent<ASPathFollower>();

        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(Grazing.Instance);

        raptys = new List<GameObject>();
        ankys = new List<GameObject>();
        closestAnky = new GameObject();
        closestRapty = new GameObject();

        // Assert default animation booleans and floats
        //anim.SetBool("isIdle", true);
        //anim.SetBool("isEating", false);
        //anim.SetBool("isDrinking", false);
        //anim.SetBool("isAlerted", false);
        //anim.SetBool("isGrazing", false);
        //anim.SetBool("isAttacking", false);
        //anim.SetBool("isFleeing", false);
        //anim.SetBool("isDead", false);
        //anim.SetFloat("speedMod", 1.0f);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
    }

    private void FOV()
    {
        raptys.Clear();
        ankys.Clear();

        foreach (Transform i in dinoView.visibleTargets)
        {
            if (i.tag == ("Rapty"))
            {
                raptys.Add(i.gameObject);
            }
        }

        foreach (Transform i in dinoView.stereoVisibleTargets)
        {
            if (i.tag == ("Rapty"))
            {
                raptys.Add(i.gameObject);
            }
        }

        float closest = 9999;

        for (int i = 0; i < raptys.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, raptys[i].transform.position);

            if (closest > distance)
            {
                closest = distance;
                closestRapty = raptys[i];
            }
        }

        foreach (Transform i in dinoView.visibleTargets)
        {
            if (i.tag == ("Anky") && i.gameObject != gameObject)
            {
                ankys.Add(i.gameObject);
            }
        }

        foreach (Transform i in dinoView.stereoVisibleTargets)
        {
            if (i.tag == ("Anky") && i.gameObject != gameObject)
            {
                ankys.Add(i.gameObject);
            }
        }

        closest = 9999;

        for (int i = 0; i < ankys.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, ankys[i].transform.position);

            if (closest > distance)
            {
                closest = distance;
                closestAnky = ankys[i];
            }
        }
    }

    public void grazing()
    {
        Debug.Log("In Grazing State");
        //anim.SetBool("isIdle", false);
        //anim.SetBool("isEating", false);
        //anim.SetBool("isDrinking", false);
        //anim.SetBool("isAlerted", false);
        //anim.SetBool("isGrazing", true);
        //anim.SetBool("isAttacking", false);
        //anim.SetBool("isFleeing", false);
        //anim.SetBool("isDead", false);
        //anim.SetFloat("speedMod", 1.0f);
        float dist2 = Vector3.Distance(transform.position, closestRapty.transform.position);

        if (dist2 <= 29)
        {
            currentState = ankyState.ALERTED;
            wander.enabled = false;
        }
        if (food < 50 && dist2 > 29)
        {
            currentState = ankyState.EATING;
            wander.enabled = false;
        }
        if (water < 80 && dist2 > 29)
        {
            currentState = ankyState.DRINKING;
            wander.enabled = false;
        }
        else
        {
            wander.enabled = true;

            ankyDist = Vector3.Distance(transform.position, closestAnky.transform.position);

            if (ankyDist > herdDistance)
            {
                wander.enabled = false;
                seeking.target = closestAnky;
                seeking.enabled = true;
            }
            else
            {
                seeking.enabled = false;
            }
        }
        //create conditions that when met, do next stuff
        //set the current state to be the next one
        //turn wander off
    }



    public void alerted()
    {
        foreach (Transform i in dinoView.visibleTargets)
        {
            if (i.tag == ("Rapty"))
            {
                float distAlert;
                distAlert = Vector3.Distance(this.transform.position, i.transform.position);
                Debug.Log("I'm alerted");
                if (distAlert >= 35)
                {
                    currentState = ankyState.GRAZING;
                    wander.enabled = true;
                    //anim.SetBool("isIdle", false);
                    //anim.SetBool("isEating", false);
                    //anim.SetBool("isDrinking", false);
                    //anim.SetBool("isAlerted", false);
                    //anim.SetBool("isGrazing", true);
                    //anim.SetBool("isAttacking", false);
                    //anim.SetBool("isFleeing", false);
                    //anim.SetBool("isDead", false);
                    //anim.SetFloat("speedMod", 1.0f);
                    Debug.Log("Not alerted, back to grazing");
                }
                else if (distAlert >= 27)
                {
                    currentState = ankyState.ALERTED;
                    wander.enabled = true;
                    //anim.SetBool("isIdle", false);
                    //anim.SetBool("isEating", false);
                    //anim.SetBool("isDrinking", false);
                    //anim.SetBool("isAlerted", true);
                    //anim.SetBool("isGrazing", false);
                    //anim.SetBool("isAttacking", false);
                    //anim.SetBool("isFleeing", false);
                    //anim.SetBool("isDead", false);
                    //anim.SetFloat("speedMod", 1.0f);
                }
                else if(distAlert <= 26)
                {
                    currentState = ankyState.FLEEING;
                    wander.enabled = true;
                    //anim.SetBool("isIdle", false);
                    //anim.SetBool("isEating", false);
                    //anim.SetBool("isDrinking", false);
                    //anim.SetBool("isAlerted", false);
                    //anim.SetBool("isGrazing", false);
                    //anim.SetBool("isAttacking", false);
                    //anim.SetBool("isFleeing", true);
                    //anim.SetBool("isDead", false);
                    //anim.SetFloat("speedMod", 1.0f);
                }
            }
        }
    }

    public void fleeing()
    {
        float dist = Vector3.Distance(transform.position, closestRapty.transform.position);

        if (dist <= 20)
        {
            currentState = ankyState.FLEEING;
            wander.enabled = false;
            //anim.SetBool("isIdle", false);
            //anim.SetBool("isEating", false);
            //anim.SetBool("isDrinking", false);
            //anim.SetBool("isAlerted", false);
            //anim.SetBool("isGrazing", false);
            //anim.SetBool("isAttacking", false);
            //anim.SetBool("isFleeing", true);
            //anim.SetBool("isDead", false);
            //anim.SetFloat("speedMod", 1.0f);
            flee.target = closestRapty;
            flee.enabled = true;
            Debug.Log("fleeing from raptor");
        }
        else if (dist >= 50)
        {
            currentState = ankyState.ALERTED;
            wander.enabled = true;
            flee.enabled = false;
            //anim.SetBool("isIdle", false);
            //anim.SetBool("isEating", false);
            //anim.SetBool("isDrinking", false);
            //anim.SetBool("isAlerted", false);
            //anim.SetBool("isGrazing", false);
            //anim.SetBool("isAttacking", false);
            //anim.SetBool("isFleeing", true);
            //anim.SetBool("isDead", false);
            //anim.SetFloat("speedMod", 1.0f);
            Debug.Log("Not fleeing but alerted");
        }
    }

    public void eating()
    {
        if (transform.position.y >= 50 && food < 100)
        {
            food += (Time.deltaTime* 2) *1.0f;
            Debug.Log(food);
        }
        else
        {
            Debug.Log("Food is full");
            currentState = ankyState.GRAZING;
        }       
    }

    public void drinking()
    {
        wander.enabled = false;
        seeking.target = Water;
        seeking.enabled = true;

        if (transform.position.y <= 36)
        {
            seeking.enabled = false;
            water += (Time.deltaTime *3) * 1.0f;
        }

        if (water >= 100)
        {
            seeking.enabled = false;
            currentState = ankyState.GRAZING;
        }       
    }

    //public void breed()
    //{
    //    float ankyDist = (transform.position - closestAnky.transform.position).magnitude;

    //    if (ankyDist > 3)
    //    {
    //        AStar.target = closestAnky;

    //        if (pathFollower.path.nodes.Count < 1 || pathFollower.path == null)
    //            pathFollower.path = AStar.path;

    //        movement(pathFollower.getDirectionVector());
    //    }
    //}

    //void move (Vector3 directionVector)
    //{
    //    directionVector *= speed * Time.deltaTime;

    //    transform.Translate(directionVector, Space.World);
    //    transform.LookAt(transform.position + directionVector);
    //}


    public void death()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    } 


    protected override void Update()
    {

        water -= (Time.deltaTime * 0.3f) * 1.0f;
        food -= (Time.deltaTime * 0.1f) * 1.0f;
        if (food <= 0 || water <= 0)
        {
            water = 0;
            food = 0;
            health -= 0.5f * Time.deltaTime; 
        }
        //Debug.Log("food");
        //Debug.Log(food);
        //Debug.Log("water");
        //Debug.Log(water);

        //if (asPath.path.nodes.Count < 1 || asPath.path == null)
        //    asPath.path = aStar.path;

        //move(asPath.getDirectionVector());

        FOV();
        if (health <= 0)
        {
            death();
        }
        
        stateMachine.Update();

        
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here



        

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        //ankyState currentState = ankyState.DEAD;

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
