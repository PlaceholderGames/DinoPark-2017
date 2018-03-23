using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using stateMachine;

public class MyAnky : Agent
{

    public StateMachine<MyAnky> stateMachine { get; set; }
    public bool switchState = false;


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
    public MyAnky myAnky;
    public float hunger = 100, thirst = 100;

    public List<Transform> Raptors;
    public List<Transform> AnkysHerd;

    private bool canBeEating = false;
    private bool canBeDrinking = false;

    public List<Transform> WaterObjects;
    public List<Transform> FoodObjects;

    public AStarSearch ankyAStar;
    public ASAgentInstance ankyASIntance;
    public ASPathFollower ankyAsPath;

    
    public Flee ankyFleeing;
    public Face ankyFacing;
    public Wander ankyWandering;
    public Seek ankySeeking;
    public Herding ankyHerding;
    public Agent agent;

    public GameObject water;
    public GameObject RaptyEnemy = null;
    public GameObject AnkyFriend = null;
    public FieldOfView ankyView;
    public Transform myTransform;
    public float weight = 1.0f;

    public float raptorDistance;
    public float ankyDistance;


    public virtual void Awake()
    {

        //myAnky = gameObject.GetComponent<MyAnky>();
    }

    //IEnumerator





    // Use this for initialization
    protected override void Start()
    {

        myAnky = gameObject.GetComponent<MyAnky>();
        agent = GetComponent<Agent>();

        stateMachine = new StateMachine<MyAnky>(this);
        
        stateMachine.ChangeState(Idle.Instance);
        
        myAnky.maxSpeed = 5.0f;

        Raptors = new List<Transform>();
        AnkysHerd = new List<Transform>();

        myTransform = this.transform;
        anim = GetComponent<Animator>();
        ankyView = GetComponent<FieldOfView>();

        ankyFleeing = GetComponent<Flee>();
        ankyFacing = GetComponent<Face>();
        ankyWandering = GetComponent<Wander>();       
        ankyHerding = GetComponent<Herding>();


        ankyAStar = GetComponent<AStarSearch>();
        ankyASIntance = GetComponent<ASAgentInstance>();
        ankyAsPath = GetComponent<ASPathFollower>();



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
        ScanRaptors();
        ScanAnkys();
        stateMachine.Update();
        StartCoroutine(LowerFoodAndThirt());        
        base.Update();
    } 
    public void ScanAnkys()
    {        
        AnkysHerd = new List<Transform>();
        float farthestAnky = 1;        
        int ankyIndex = 0;
        GameObject anky = null;

        for (int i = 0; i < ankyView.visibleTargets.Count; i++) // for loop that checks all visible targets
        {
            if (ankyView.visibleTargets[i].gameObject.tag == "Anky")    // if target is called anky store it to a list
            {
                AnkysHerd.Add(ankyView.visibleTargets[i]);
            }
        }
        for (int i = 0; i < AnkysHerd.Count; i++)
        {
            ankyDistance = Vector3.Distance(myTransform.position, AnkysHerd[i].position);   
            if (ankyDistance > farthestAnky)   // if the anky distance is greater than the current furthest anky 
            {
                farthestAnky = ankyDistance;   
                ankyIndex = i;
                anky = AnkysHerd[ankyIndex].gameObject; // store that anky as the new furthest
            }
        }
        if (anky != null)
        {
            if (anky.gameObject.tag == "Anky")  // is anky isnt empty and has a tag of anky store as the anky firend
            {
                AnkyFriend = anky;
            }
            else
            {
                AnkyFriend = null;
            }
        }
    }

    public void ScanRaptors()
    {
        float distance = -1;
        Raptors = new List<Transform>();
        float closestRaptor = 200;
        int dinoIndex = 0;
        GameObject dino = null;
        for (int i = 0; i < ankyView.visibleTargets.Count; i++)     // check all target in viewable range of anky
        {
            if (ankyView.visibleTargets[i].gameObject.tag == "Rapty")   // if rapty store in raptors list
            {
                Raptors.Add(ankyView.visibleTargets[i]);
            }
        }
        if (Raptors.Count > 0)
        {
            for (int i = 0; i < Raptors.Count; i++)
            {
                distance = Vector3.Distance(myTransform.position, Raptors[i].position);

                if (raptorDistance < closestRaptor) // if raptor distance is less than the closest raptor
                {
                    closestRaptor = raptorDistance; 
                    dinoIndex = i;
                    dino = Raptors[dinoIndex].gameObject;   // store that raptor as the new closest raptor
                }
            }
            RaptyEnemy = dino;      // set enemy raptor to game object dino
            raptorDistance = distance;
        }
        else
        {
            RaptyEnemy = null;
            raptorDistance = -1.0f;
        }
    }        
    public IEnumerator LowerFoodAndThirt()
    {
        yield return new WaitForSeconds(2);
        thirst = thirst - 0.1f;
        hunger = hunger - 0.1f;
    }  
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
