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

    private Animator anim;
    raptyState fsm;
    private FieldOfView FOV;
    private Transform TempaT;
    private Transform nearest;
    private GameObject NearestPoint;
    private float NearestDistance;
    private Pursue pursue;
    private WaterPursue waterpursue;
    private Wander wander;
    private Flee flee;
    private Face face;
    private FlipMe flipme;
    private bool busy;
    private GameObject TheAnky;
    private GameObject[] AvailableAnkys;
    public List<Transform> ankysInSight = new List<Transform>();
    public float health = 100, energy = 20, thirst = 25;
    private bool actuallyDrinking = false, ballMoved = false, stateOnce = false;
    public Transform ball;
    public Transform deadAnky;
    GameObject theBall;

    protected override void Start()
    {
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

        FOV = GetComponent<FieldOfView>();
        pursue = GetComponent<Pursue>();
        wander = GetComponent<Wander>();
        flee = GetComponent<Flee>();
        face = GetComponent<Face>();
        flipme = GetComponent<FlipMe>();
        waterpursue = GetComponent<WaterPursue>();
        AvailableAnkys  = GameObject.FindGameObjectsWithTag("Anky");
        fsm = raptyState.IDLE;
        base.Start();
        StartCoroutine(CalculateLife(0.2f));
    }

    protected override void Update()
    {
        Debug.Log(fsm.ToString());
        if(health <= 0)
        {
            fsm = raptyState.DEAD;
        }

        // Idle - should only be used at startup
        if (fsm == raptyState.IDLE)
        {
            stateOnce = false;
            busy = false;
            pursue.enabled = false;
            face.enabled = false;
            wander.enabled = false;
            flee.enabled = false;
            anim.SetBool("isIdle", false);
            anim.SetBool("isAlerted", true);
            fsm = raptyState.ALERTED;


        }
        // Eating - requires a box collision with a dead dino
        else if (fsm == raptyState.EATING)
        {
            if(!busy)
            {

                busy = true;
                StartCoroutine(Eat());
            }
        }
        // Drinking - requires y value to be below 32 (?)
        else if (fsm == raptyState.DRINKING)
        {
            if (!actuallyDrinking)
            {
                if (!ballMoved)
                {
                    if (this.transform.position.y > 37)
                    {
                        //theBall = GameObject.FindGameObjectWithTag("ball");
                        //theBall.transform.SetPositionAndRotation(this.transform.position, Quaternion.identity);
                        GameObject[] WaterPoints = GameObject.FindGameObjectsWithTag("Water");

                        for(int i = 0; i < WaterPoints.Length; i++)
                        {
                            if (i == 0)
                            {
                                NearestPoint = WaterPoints[i];
                                NearestDistance = Vector3.Distance(NearestPoint.transform.position, this.transform.position);
                            }
                            else
                            {
                                float Distance = Vector3.Distance(WaterPoints[i].transform.position, this.transform.position);
                                if (Distance < NearestDistance)
                                {
                                    NearestPoint = WaterPoints[i];
                                    NearestDistance = Distance;
                                }
                            }
                        }
                        Debug.Log(NearestPoint);
                        waterpursue.target = NearestPoint;
                        waterpursue.newTarget();
                        wander.enabled = false;
                        waterpursue.enabled = true;
                        ballMoved = true;
                    }
                }


                if (this.transform.position.y < 37)
                {
                    actuallyDrinking = true;
                    ballMoved = false; 
                    waterpursue.enabled = false;
                    anim.SetBool("isDrinking", true);
                    anim.SetBool("isHunting", false);
                    anim.SetBool("isAlerted", false);

                    StartCoroutine(Drink());
                }

            }
        }
        // Alerted - up to the student what you do here
        else if (fsm == raptyState.ALERTED)
        {
            if (!stateOnce)
            {

                Debug.Log(wander.enabled.ToString());
                pursue.enabled = false;
                face.enabled = false;
                wander.enabled = true;
                Debug.Log(wander.enabled.ToString());
                stateOnce = true;
            }
            if(thirst < 20)
            {
                stateOnce = false;
                anim.SetBool("isDrinking", true);
                anim.SetBool("isAlerted", false);
                fsm = raptyState.DRINKING;
            }

            if (FOV.visibleTargets.Count > 1)
            {
                foreach (Transform Dino in FOV.visibleTargets)
                {
                    if (Dino.tag == "Anky" && Dino.gameObject.GetComponent<Agent>().alive == true)
                    {
                        ankysInSight.Add(Dino);
                    }
                }
                //getNearest();
                if(ankysInSight.Count > 0)
                {
                    TheAnky = ankysInSight[0].gameObject;
                    ankysInSight.Clear();
                    pursue.target = TheAnky;
                    pursue.newTarget();
                    stateOnce = false;
                    anim.SetBool("isHunting", true);
                    anim.SetBool("isAlerted", false);
                    fsm = raptyState.HUNTING;
                }

            }
        }
        // Hunting - up to the student what you do here
        else if (fsm == raptyState.HUNTING)
        {
            if(!stateOnce)
            {
                wander.enabled = false;
                pursue.enabled = true;
                //face.enabled = true;
                stateOnce = true;
                StartCoroutine(GiveUp());
            }
            float DinoDist = Vector3.Distance(pursue.target.transform.position, this.transform.position);
            Debug.Log(DinoDist);
            if(DinoDist < 5.0)
            {
                anim.SetBool("isHunting", false);
                anim.SetBool("isAttacking", true);
                stateOnce = false;
                fsm = raptyState.ATTACKING;
            }
        }
        //Attacking state
        else if (fsm == raptyState.ATTACKING)
        {
            if(!stateOnce)
            {
                stateOnce = true;
                pursue.enabled = false;
                Vector3 AnkyPos = TheAnky.transform.position;
                Agent ankyAgent = TheAnky.gameObject.GetComponent<Agent>();
                if(ankyAgent != null) { Debug.Log("Found it!"); ankyAgent.TimeToDie(); }
                TheAnky.transform.Rotate(0.0f, 0.0f, 180.0f);
                //Instantiate(deadAnky, AnkyPos, new Quaternion(0.0f, 0.0f, 180.0f, 0.0f));
                stateOnce = false;
                fsm = raptyState.EATING;

            }
        }

        // Fleeing - up to the student what you do here
        else if (fsm == raptyState.FLEEING)
        {
            if (!stateOnce)
            {
                wander.enabled = false;
                pursue.enabled = false;
                flee.enabled = true;
                stateOnce = true;
                StartCoroutine(GiveUp());
            }
        }
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
        else if (fsm == raptyState.DEAD)
        {
            if (!stateOnce)
            {
                wander.enabled = false;
                pursue.enabled = false;
                flee.enabled = false;
                waterpursue.enabled = false;
                anim.SetBool("isDead", true);
                flipme.Flip();
                this.enabled = false;
            }
        }
        base.Update();
    }

    IEnumerator Eat()
    {
        yield return new WaitForSeconds(5.0f);

        energy = 100;
        busy = false;
        anim.SetBool("isEating", false);
        anim.SetBool("isAlerted", true);
        stateOnce = false;
        fsm = raptyState.ALERTED;

    }

    IEnumerator Drink()
    {
        yield return new WaitForSeconds(5.0f);

        thirst = 100;
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", true);
        fsm = raptyState.ALERTED;

        actuallyDrinking = false;
    }

    IEnumerator GiveUp()
    {
        yield return new WaitForSeconds(20.0f);
        if(fsm == raptyState.HUNTING || fsm == raptyState.FLEEING)
        {
            stateOnce = false;
            anim.SetBool("isHunting", true);
            anim.SetBool("isFleeing", true);
            anim.SetBool("isAlerted", true);
            fsm = raptyState.ALERTED;
        }

    }

    IEnumerator CalculateLife(float delay)
    {
        while (true)
        {
            //WaitForSeconds makes this tick occur every delay seconds
            yield return new WaitForSeconds(delay);

            //Decrement thirst on each tick if raptor is not drinking
            //Did not use drinking state as dinosaur can be in drinking state but not actually drinking due to height restrictions
            if (!actuallyDrinking && thirst > 0) { thirst -= 0.1f; }
        
            //Energy goes down quickly if in the hunting state as the rapty is using more energy
            if(energy > 0)
            {
                if (fsm == raptyState.HUNTING) { energy -= 0.2f; }
                else if (fsm == raptyState.EATING) { }
                else { energy -= 0.1f; }
            }


            //When the rapty's thirst or energy gets to 0, his health starts to tick down. The effect is compounded if he is hungry and thirsty
            if(thirst <= 0 && !actuallyDrinking) { health -= 0.5f; }
            if(energy <= 0 && fsm != raptyState.EATING) { health -= 0.5f; }
            if(thirst > 80 && energy > 80) { health += 0.2f; }
        }
    }

    void getNearest()
    {
        for (int i = 0; i < ankysInSight.Count; i++)
        {
            TempaT = ankysInSight[i];
            if (i == 0)
            {
                nearest = TempaT;
            }
            else
            {
                if ((TempaT.transform.position-transform.position).magnitude < (nearest.transform.position - transform.position).magnitude)
                {
                    nearest = TempaT;
                }
            }
        }
        pursue.target = nearest.gameObject;
    }

    void onColliderEnter(Collider other)
    {
        if (other.tag == "Anky")
        {
            if(other.GetComponent<Agent>().alive)
            {
                anim.SetBool("isHunting", false);
                anim.SetBool("isAttacking", true);
                fsm = raptyState.ATTACKING;
            }

        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
