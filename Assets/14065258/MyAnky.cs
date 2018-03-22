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
    public float health = 100;
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
        ankyList = GameObject.FindGameObjectsWithTag("Anky");



        if (GameObject.Find("WaterSphereAnky") != null)
        {
            waterDrinkLocation = GameObject.Find("WaterSphereAnky");
        }
        if (MapGrid.FindObjectOfType<MapGrid>() != null)
        {
            this.GetComponent<AStarSearch>().mapGrid = MapGrid.FindObjectOfType<MapGrid>();
        }
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

        currentState = "Idle";

    }

    protected override void Update()
    {
        float distance;
        waterLevel -= 2 * Time.deltaTime;
        hungryLevel -= 2 * Time.deltaTime;

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

        if (!leader)
        {

            distance = Vector3.Distance(this.transform.position, ankyLeader.transform.position);
            if (distance > 20)
            {
                //GetComponent<Wander>().enabled = false;
                //speed = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, ankyLeader.transform.position, speed);

                Debug.Log("help");
                //GetComponent<Flee>().enabled = true;
            }

        }



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

    void IdleState()
    {
        float distance;
        gameObject.GetComponent<Agent>().enabled = true;
        gameObject.GetComponent<Wander>().enabled = true;
        if (waterLevel < 20)
        {
            currentState = "Drinking";
        }
        if (hungryLevel < 20)
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
        //_owner.transform.transform.localRotation = new Quaternion(0, 0, 0, 1);

        if (Vector3.Distance(waterDrinkLocation.transform.position, transform.position) < 25 && waterLevel < 100)
        {
            waterLevel += 20 * Time.deltaTime;
        }
        else if (waterLevel >= 100)
        {
            currentState = "Idle";
        }
    }

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
        //_owner.transform.transform.localRotation = new Quaternion(0, 0, 0, 1);

        if (Vector3.Distance(waterDrinkLocation.transform.position, transform.position) < 25 && hungryLevel < 100)
        {
            hungryLevel += 20 * Time.deltaTime;
        }
        else if (hungryLevel >= 100)
        {
            currentState = "Idle";
        }
    }

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

    private void move(Vector3 directionVector, MyAnky _owner)
    {
        directionVector *= speed * Time.deltaTime;
        _owner.transform.Translate(directionVector, Space.World);
        _owner.transform.LookAt(_owner.transform.position + directionVector);

    }
}
