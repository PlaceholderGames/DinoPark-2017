using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Josh
using StateStuff;
using UnityEditor;

public class MyRapty : MonoBehaviour
{
    //Josh
    [HideInInspector]
    public FieldOfView dinoView;
    //[HideInInspector]
    //public Wander dinoWander;
    //[HideInInspector]
    //public Face dinoFace;
    //[HideInInspector]
    //public Pursue dinoPursue;
    //[HideInInspector]
    //public AgentBehaviour dinoAgent;
    //[HideInInspector]
    public GameObject waterDrink;

    public GameObject controller;
    public GameObject raptor;
    public GameObject dinoPrefab;
    public List<GameObject> allRaptors = new List<GameObject>();
    public GameObject[] raptorArray;
    private AStarSearch aStarScript;
    private ASPathFollower pathFollowerScript;

    public int targetDinoLocation;
    public StateMachine<MyRapty> stateMachine { get; set; }
    public bool switchState = false;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    [HideInInspector]
    public float moveSpeed = 3f;
    [HideInInspector]
    public float rotSpeed = 100f;

    //[HideInInspector]
    public float waterLevel = 100f;
    //[HideInInspector]
    public float hungerLevel = 100f;
    //[HideInInspector]
    public float energyLevel = 100f;
    public bool isSleeping = false;
    public float distance = 0;
    public bool breeding = false;




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
    public Animator anim;
    //public raptyState currentState;
    
    // Use this for initialization
    protected void Start()
    {
        foreach (GameObject item in raptorArray)
        {
            allRaptors.Add(item);
        }
        if (GameObject.Find("WaterSphere") != null)
        {
            waterDrink = GameObject.Find("WaterSphere");
        } 
        if (GameObject.Find("Rapty") != null)
        {
            dinoPrefab = GameObject.Find("Rapty");
        }
        if (MapGrid.FindObjectOfType<MapGrid>() != null)
        {
            this.GetComponent<AStarSearch>().mapGrid = MapGrid.FindObjectOfType<MapGrid>();
        }
        this.GetComponent<AStarSearch>().target = this.gameObject;

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
        //base.Start();
        //Josh
        dinoView = GetComponent<FieldOfView>();
        aStarScript = GetComponent<AStarSearch>();
        pathFollowerScript = GetComponent<ASPathFollower>();



        stateMachine = new StateMachine<MyRapty>(this);
        stateMachine.ChangeState(IdleState.Instance);

    }

    private void Update()
    {
        if (dinoView.visibleTargets.Count != 0)
        {
            for (int i = 0; i < dinoView.visibleTargets.Count; i++)
            {
                if (dinoView.visibleTargets[i].CompareTag("Rapty"))
                {
                    distance = Vector3.Distance(transform.position, dinoView.visibleTargets[i].transform.position);
                    if (distance > 20)
                    {
                        aStarScript.target = dinoView.visibleTargets[i].gameObject;

                        if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
                        {
                            pathFollowerScript.path = aStarScript.path;
                        }
                        move(pathFollowerScript.getDirectionVector(), this);
                    }
                }
            }

        }

        energyLevel += 5 * Time.deltaTime;
        //waterLevel -= 1 * Time.deltaTime;
        //hungerLevel -= 0.5f * Time.deltaTime;
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        if (stateMachine != null)
        {
                    stateMachine.Update();
        }

        if (transform.position.y < 32)
        {
            transform.position = new Vector3(transform.position.x,150,transform.position.z);
        }



        //Update();
    }

    public void movement(MyRapty _owner, float movespeed)
    {
        _owner.energyLevel -= (moveSpeed / 2) * Time.deltaTime;
        if (Random.Range(0, 5) < 1)
            Wander();
        if (_owner.isWandering == false)
        {
            _owner.StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            _owner.transform.Rotate(_owner.transform.up * Time.deltaTime * _owner.rotSpeed);
        }
        if (isRotatingLeft == true)
        {
            _owner.transform.Rotate(_owner.transform.up * Time.deltaTime * -_owner.rotSpeed);
        }
        if (isWalking == true)
        {
            _owner.transform.position += _owner.transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 1); // times to rotate
        int rotateWait = Random.Range(1, 5); //waiting between rotate
        int rotateLorR = Random.Range(1, 100); //either left or right. random
        int walkWait = Random.Range(1, 4); // inbetween walking
        int walkTime = Random.Range(5, 10); // walking for how long

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);

        if (rotateLorR % 2 < 5)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR % 2 > 5)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;

    }

    public void breed(Vector3 pos)
    {
        Instantiate(dinoPrefab, pos, Quaternion.identity);
    }

    void move(Vector3 directionVector, MyRapty _owner)
    {
        directionVector *= moveSpeed * Time.deltaTime;
        if (_owner.dinoView.visibleTargets.Count == 0 && _owner.dinoView.stereoVisibleTargets.Count == 0)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }
        _owner.transform.Translate(directionVector, Space.World);
        _owner.transform.LookAt(_owner.transform.position + directionVector);

    }
    /*
    protected void LateUpdate()
    {
       // LateUpdate();
    } */
}
