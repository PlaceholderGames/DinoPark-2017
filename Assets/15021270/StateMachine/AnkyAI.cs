using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkyAI : MonoBehaviour {

    private FieldOfView view;
    private FleeRotate fleeR;
    private Wander wander;
    private Agent agent;
    private CollisionCheckAnky collision;

    public GameObject pred;

    private float fleeingTime;
    private float decayTime;
    public int health = 100;
    public int meat = 100;

    private bool enemyFound = false;
    private bool flee = false;
    public bool dead = false;
    public bool noMeat = false;

	// Use this for initialization
	void Start () {
        view = GetComponent<FieldOfView>();
        fleeR = GetComponent<FleeRotate>();
        wander = GetComponent<Wander>();
        agent = GetComponent<Agent>();
        collision = GetComponent<CollisionCheckAnky>();
	}
	
	// Update is called once per frame
	void Update () {
        
        
        //Example of flee, idle and dead states without a state machine.  Chaos!!!!
        if (dead == false)
        {
            if(health <= 0)
            {
                dead = true;
            }

            
            if (enemyFound == true && Vector3.Distance(transform.position, pred.transform.position) < 200)
            {
                flee = true;
            }
            else
            {
                foreach (Transform i in view.visibleTargets)
                {
                    if (i.tag == "Rapty")
                    {
                        enemyFound = true;
                        pred = i.gameObject;
                    }
                }
            }

            if (flee)
            {
                fleeingTime += Time.deltaTime;
                if (fleeingTime >= 10)
                {
                    flee = false;
                }

                fleeR.target = pred;

                //Enable flee, disable wander
                wander.enabled = false;
                fleeR.enabled = true;
                agent.maxSpeed = 10;

            }
            else
            {
                //Disable flee, enable wander
                fleeingTime = 0;
                wander.enabled = true;
                fleeR.enabled = false;
                agent.maxSpeed = 2;

            }
        }
        else
        {
            wander.enabled = false;
            fleeR.enabled = false;
            agent.maxSpeed = 0;
            //Debug.Log("Anky is dead");

            //If there is no meat left then destroy it
            decayTime += Time.deltaTime;
            if (decayTime >= 30)
            {
                noMeat = true;
                //Destroy(this.gameObject);
            }
            
    
        }
	}
}
