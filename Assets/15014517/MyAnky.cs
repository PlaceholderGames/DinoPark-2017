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

    //state scripts
    public Flee ankyFleeing;
    public Face ankyFacing;
    public Wander ankyWandering;
    public Seek ankySeeking;
    public Herding ankyHerding;
    public Agent agent;

    public GameObject water;
    public GameObject Enemy = null;
    public GameObject Friend = null;
    public FieldOfView ankyView;
    public Transform myTransform;
    public float weight = 1.0f;




    public virtual void Awake()
    {

        //myAnky = gameObject.GetComponent<MyAnky>();
    }

    //IEnumerator





    // Use this for initialization
    protected override void Start()
    {

        myAnky = gameObject.GetComponent<MyAnky>();
        stateMachine = new StateMachine<MyAnky>(this);
        // set state imediately to idle
        stateMachine.ChangeState(Idle.Instance);
        //set state to grazing
        stateMachine.ChangeState(Grazing.Instance);



        



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


        //wait();

        // Idle - should only be used at startup

        //Debug.Log(myTransform.position);


        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }























    public float raptorDistance;
    public float ankyDistance;

    public void ScanAnkys()
    {
        
        AnkysHerd = new List<Transform>();
        float closeAnky = 1;
        //Vector3 farAnky = new Vector3(4000, 4000, 4000);
        int ankyIndex = 0;
        GameObject anky = null;

        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            if (ankyView.visibleTargets[i].gameObject.tag == "Anky")
            {
                AnkysHerd.Add(ankyView.visibleTargets[i]);
            }
        }


        for (int i = 0; i < AnkysHerd.Count; i++)
        {

            ankyDistance = Vector3.Distance(myTransform.position, AnkysHerd[i].position);

            if (ankyDistance > closeAnky)
            {
                closeAnky = ankyDistance;
                ankyIndex = i;
                anky = AnkysHerd[ankyIndex].gameObject;

            }
        }



        if (anky != null)
        {
            if (anky.gameObject.tag == "Anky")
            {
                Friend = anky;
            }
            else
            {
                Friend = null;

            }

        }

    }




    public void ScanRaptors()
    {
        float distance = -1;
        Raptors = new List<Transform>();
        float closeDinosaur = 200;
        int dinoIndex = 0;
        GameObject dino = null;
        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            if (ankyView.visibleTargets[i].gameObject.tag == "Rapty")
            {
                Raptors.Add(ankyView.visibleTargets[i]);
            }
        }

        for (int i = 0; i < ankyView.stereoVisibleTargets.Count; i++)
        {
            if (ankyView.stereoVisibleTargets[i].gameObject.tag == "Rapty")
            {
                Raptors.Add(ankyView.stereoVisibleTargets[i]);
            }
        }

        if (Raptors.Count > 0)
        {

            for (int i = 0; i < Raptors.Count; i++)
            {

                distance = Vector3.Distance(myTransform.position, Raptors[i].position);

                if (raptorDistance < closeDinosaur)
                {
                    closeDinosaur = raptorDistance;
                    dinoIndex = i;
                    dino = Raptors[dinoIndex].gameObject;

                }
            }
            Enemy = dino;
            raptorDistance = distance;
        }


        else
        {
            Enemy = null;
            raptorDistance = -1.0f;
        }

    }

   
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.25f);
    }
    public IEnumerator LowerFoodAndThirt()
    {
        yield return new WaitForSeconds(2);
        thirst = thirst - 0.1f;
        hunger = hunger - 0.1f;
    }
    public IEnumerator Wait3()
    {
        yield return new WaitForSeconds(0.5f);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
