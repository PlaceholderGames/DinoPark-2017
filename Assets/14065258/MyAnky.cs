using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAnky : Agent
{
    private FieldOfView dinoView;
    private AStarSearch aStarScript;
    private ASPathFollower pathFollowerScript;
    public GameObject[] ankyList;
    public GameObject waterDrinkLocation;
    public GameObject ankyLeader;
    public float waterLevel = 100;
    public float hungryLevel = 100;
    public float speed = 5;
    public bool leader;

    public string currentState;

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
        // -- Josh --
        // Will get a list of all the Anky's in the world currently. 
        // and will also set up all the objects that it can use.
        ankyList = GameObject.FindGameObjectsWithTag("Anky");

        // Sets the Anky water sphere if it hasnt been set
        if (GameObject.Find("WaterSphereAnky") != null)
        {
            waterDrinkLocation = GameObject.Find("WaterSphereAnky");
        }
        // will get the Map grid set if it hasnt been
        if (MapGrid.FindObjectOfType<MapGrid>() != null)
        {
            this.GetComponent<AStarSearch>().mapGrid = MapGrid.FindObjectOfType<MapGrid>();
        }
        // and if the flee target hasnt been set it will set it to itself, 
        // this will get changed later when it flee's anyway. 
        if (this.GetComponent<Flee>().target == null)
        {
            this.GetComponent<Flee>().target = this.gameObject;
        }

        aStarScript = GetComponent<AStarSearch>();
        pathFollowerScript = GetComponent<ASPathFollower>();
        dinoView = GetComponent<FieldOfView>();

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

        // -- Josh --
        // go into the Idle state at first
        currentState = "Idle";

    }




    protected override void Update()
    {
        float distance;
        waterLevel -= 2 * Time.deltaTime;
        hungryLevel -= 2 * Time.deltaTime;


        // -- Josh --
        // The update state will drain the water and hungry levels and keep
        // control of all the states. 
        switch (currentState)
        {
            case "Idle":
                IdleState();
                break;
            case "Drinking":
                DrinkingState();
                break;
            case "Hungry":
                HungryState();
                break;
            case "Fleeing":
                FleeingState();
                break;
            default:
                break;
        }

        // If its not the leader ( set in the editor ) it will move to keep
        // itself close to the leader.
        if (!leader)
        {
            distance = Vector3.Distance(this.transform.position, ankyLeader.transform.position);
            if (distance > 20)
            {
                transform.position = Vector3.MoveTowards(transform.position, ankyLeader.transform.position, speed);
            }
        }


        // and if it falls out of the world for whatever reason, it will teleport back
        if (transform.position.y < 32)
        {
            transform.position = new Vector3(transform.position.x, 150, transform.position.z);
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

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    // -- Josh --
    // Will deal damage to the Anky when called by the Rapty
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


    // Will be the default state for the Anky.
    // When a certain condition is matched, it will switch to that state.
    void IdleState()
    {
        gameObject.GetComponent<Agent>().enabled = true;
        gameObject.GetComponent<Wander>().enabled = true;
        Debug.Log("thinking");
        if (waterLevel < 20)
        {
            currentState = "Drinking";
        }
        if (hungryLevel <20)
        {
            currentState = "Hungry";
        }
        if (dinoView.visibleTargets.Count != 0)
        {
            for (int i = 0; i < dinoView.visibleTargets.Count; i++)
            {
                if (dinoView.visibleTargets[i].CompareTag("Rapty"))
                {
                    currentState = "Fleeing";
                   
                }
            }

        }
    }

    // -- Josh --
    // Will A* to the cloest water grid edge from the mapGrid we created
    // when in range of the orb, it will restore its water level before 
    // going back to Idle
    void DrinkingState()
    {
        gameObject.GetComponent<Wander>().enabled = false;
        gameObject.GetComponent<Agent>().enabled = false;
        waterDrinkLocation.transform.position = aStarScript.mapGrid.getEdgeWater(this.gameObject).position;
        aStarScript.target = new GameObject();
        aStarScript.target = waterDrinkLocation.gameObject;
        if (pathFollowerScript.path.nodes.Count < 1 ||
            pathFollowerScript.path == null)
        {
            pathFollowerScript.path = aStarScript.path;
        }
        move(pathFollowerScript.getDirectionVector(), this);

        if (Vector3.Distance(waterDrinkLocation.transform.position, transform.position) < 25 && waterLevel < 100)
        {
            waterLevel += 20 * Time.deltaTime;
        }
        else if (waterLevel >= 100)
        {
            currentState = "Idle";
        }
    }


    // -- Josh -- 
    // It will use the MapGrid script again to get the closest place it can get food from
    // when in range, its hunger level will go up until it switches state.
    void HungryState()
    {
        gameObject.GetComponent<Wander>().enabled = false;
        gameObject.GetComponent<Agent>().enabled = false;
        waterDrinkLocation.transform.position = aStarScript.mapGrid.getFood(this.gameObject).position;
        aStarScript.target = new GameObject();
        aStarScript.target = waterDrinkLocation.gameObject;

        if (pathFollowerScript.path.nodes.Count < 1 ||
            pathFollowerScript.path == null)
        {
            pathFollowerScript.path = aStarScript.path;
        }
        move(pathFollowerScript.getDirectionVector(), this);

        if (Vector3.Distance(waterDrinkLocation.transform.position, transform.position) < 25 && hungryLevel < 100)
        {
            hungryLevel += 20 * Time.deltaTime;
        }
        else if (hungryLevel >= 100)
        {
            currentState = "Idle";
        }
    }

    // if it can still see any dino or the dino's it can see arent Rapty's it will 
    // go back to idle, otherwise it will put that dino as its flee target and start to run away from it. 
    void FleeingState()
    {
        if (dinoView.visibleTargets.Count != 0)
        {
            for (int i = 0; i < dinoView.visibleTargets.Count; i++)
            {
                if (!dinoView.visibleTargets[i].CompareTag("Rapty"))
                {
                    currentState = "Idle";
                }
                else
                {
                    GetComponent<Wander>().enabled = false;
                    GetComponent<Flee>().target = dinoView.visibleTargets[i].gameObject;
                    GetComponent<Flee>().enabled = true;
                }
            }
        }
        else
        {
            currentState = "Idle";
        }
    }


    // used for A*
   private void move(Vector3 directionVector, MyAnky _owner)
    {
        directionVector *= speed * Time.deltaTime;
        _owner.transform.Translate(directionVector, Space.World);
        _owner.transform.LookAt(_owner.transform.position + directionVector);

    }
}
