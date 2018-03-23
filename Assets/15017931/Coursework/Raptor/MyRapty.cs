using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateMachine;


public class MyRapty : Agent
{

    public enum raptorState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on y value of the object to denote grass level
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        HUNTING,    // Moving with the intent to hunt
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };


    [Header("15017931 - Raptor")]
    public raptorState currentState = raptorState.IDLE;
    public Agent myAgent;
    public dinoStats myStats;
    public GameObject myFoodTarget;
    public GameObject myWaterTarget;
    [Space(10)]

    // list of objects we want to stay away from
    //contains the tags of objects that we will check are in our FOV
    [Header("Friend/Foe Tag Lists")]
    public string[] predators = { "" };
    public string[] friends = { "Rapty" };
    public string[] prey = { "Anky" };
    [Space(10)]


    [Header("Vision")]
    public List<Transform> dinosInVision;
    public List<Transform> predatorsInRange;
    public List<GameObject> food;
    public List<Transform> preyInRange;
    public List<Transform> friendsInRange;
    public float herdingRange = 20;

    private FieldOfView eyes;
    public Vector3 averageTargetPos = Vector3.zero;
    [Space(10)]

    //Behaviour scripts
    [Header("Behaviour Scripts")]
    public Flee fleeBehaviourScript;
    public Wander wanderBehaviourScript;
    public Seek seekBehaviourScript;
    public AStarSearch aStarScript;
    public ASPathFollower pathFollowerScript;
    [Space(10)]

    //If we are attacked 
    [Header("Other")]
    [Header("What We Gain")]
    public int drink = 10;
    public int eat = 20;
    public float timer = 100;
    [Header("What We Lose")]
    public float energyLossPerSecond = 1f;
    public float healthLossPerSecond = 1f;
    public float hungerLossPerSecond = 1f;
    public float thirstLossPerSecond = 1f;
    [Space(10)]



    [Header("Combat")]
    public int attackRange = 5;

    [Header("if We are Hit")]
    public int headDamage = 10;
    public int bodyDamage = 30;
    public int tailDamage = 2;

    [Header("if We Hit")]
    public int tailImpact = 10;
    public int headImpact = 20;
    [Space(10)]


    [Header("Death")]
    public int decayAmmount = 10;


    private Animator anim;

    // Use this for initialization
    protected override void Start()
    {
        fleeBehaviourScript = GetComponent<Flee>();
        wanderBehaviourScript = GetComponent<Wander>();
        aStarScript = GetComponent<AStarSearch>();
        pathFollowerScript = GetComponent<ASPathFollower>();
        seekBehaviourScript = GetComponent<Seek>();
        aStarScript = GetComponent<AStarSearch>();

        currentState = raptorState.IDLE;

        //Get our field of view script
        eyes = GetComponent<FieldOfView>();
        dinosInVision = new List<Transform>();
        predatorsInRange = new List<Transform>();
        preyInRange = new List<Transform>();
        friendsInRange = new List<Transform>();
        food = new List<GameObject>();

        myFoodTarget = new GameObject("foodTarget");        
        myWaterTarget = new GameObject("waterTarget");

        //aStarScript.target = this.gameObject;
        aStarScript.enabled = false;

        //set dino stats
        myStats.speed = 15;
        myStats.turnSpeed = 5;
        myStats.health = 100;
        myStats.hunger = 100;
        myStats.thirst = 100;
        myStats.energy = 100;
        myStats.deathEnergy = 100;

        fleeBehaviourScript.enabled = false;
        wanderBehaviourScript.enabled = true;

        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);

        base.Start();

    }

    protected override void Update()
    {
        updateStats();
        decideStateBehaviour();
       
        
        base.Update();
    }

    /// <summary>
    /// Check latest positions after each physics update
    /// </summary>
    protected override void LateUpdate()
    {
        blink();
        base.LateUpdate();
    }


    /// <summary>
    /// Causes dino stats to update over time
    /// </summary>
    void updateStats()
    {
        myStats.thirst -= thirstLossPerSecond * Time.deltaTime;
        myStats.hunger -= hungerLossPerSecond * Time.deltaTime;

        //Check if we have no health (DEAD)
        if (myStats.health <= 0)
        {
            currentState = raptorState.DEAD;
        }
       
        //if we have no energy left (Lose Health)
        else if (myStats.energy <= 0)
        {
            Debug.Log("depleating health");
            myStats.health -= healthLossPerSecond * Time.deltaTime;
        }
        //If we are hungry and thirsty 
        else if (myStats.hunger <= 0 && myStats.thirst <= 0)
        {
            myStats.hunger = 0;
            myStats.thirst = 0;
            Debug.Log("depleating energy");
            myStats.energy -= energyLossPerSecond * Time.deltaTime;
        }
        else if (myStats.hunger > 0 || myStats.thirst > 0)
        {
            //if we need to replenish our energy
            if (myStats.energy < 100)
            {
                //if we can use our hunger
                if (myStats.hunger > 0)
                {
                    Debug.Log("depleating hunger");
                    myStats.energy += hungerLossPerSecond * Time.deltaTime;
                }
                //if we can use our thirst
                if (myStats.thirst > 0)
                {
                    Debug.Log("depleating thirst");
                    myStats.thirst += thirstLossPerSecond * Time.deltaTime;
                }
            }
        }
        if(myStats.energy > 0 || myStats.health >= 0)
        {
            if (myStats.energy <= 100)
                myStats.energy += energyLossPerSecond * Time.deltaTime;
        }
    }


    /// <summary>
    /// Method that uses current state to determin active behaviour for 
    /// dino
    /// </summary>
    void decideStateBehaviour()
    {
        if (currentState == raptorState.IDLE)
            idleStateBehaviour();
        else if (currentState == raptorState.EATING)
            eatingStateBehaviour();
        else if (currentState == raptorState.DRINKING)
            drinkingStateBehaviour();
        else if (currentState == raptorState.FLEEING)
            fleeStateBehaviour();
        else if (currentState == raptorState.ALERTED)
            alertedStateBehaviour();
        else if (currentState == raptorState.ATTACKING)
            attackStateBehaviour();
        else if (currentState == raptorState.HUNTING)
            huntingStateBehaviour();
        else if (currentState == raptorState.DEAD)
            deadStateBehaviour();
    }


    void idleStateBehaviour()
    {
        Debug.Log("Idle");
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);;
   
    }

    void eatingStateBehaviour()
    {
        if(preyInRange.Count > 0)
        {
            foreach (Transform i in preyInRange)
            {
                if(i.gameObject.tag == "Anky")
                {
                    if(i.GetComponent<MyAnky>().currentState == MyAnky.ankyState.DEAD)
                    {
                        Vector3.MoveTowards(this.gameObject.transform.position, i.gameObject.transform.position, 10);

                        if (Vector3.Distance(this.gameObject.transform.position, i.gameObject.transform.position) <= 5)
                        {
                           
                            //Eat Anky
                           myStats.hunger += i.GetComponent<MyAnky>().decay(10);
                        }
                    }
                }
            }
            currentState = raptorState.ALERTED;
        }
    }
    void drinkingStateBehaviour()
    {

    }
    void huntingStateBehaviour()
    {

    }
    void fleeStateBehaviour()
    {

    }
    void attackStateBehaviour()
    {

    }
    void alertedStateBehaviour()
    {

    }
    void deadStateBehaviour()
    {
        //if we have no energy left
        if (myStats.deathEnergy <= 0)
        {
            Destroy(this.gameObject);
        }
        //if we have energy left
        else
        {
            myStats.deathEnergy -= 10 * Time.deltaTime;
        }

    }


    /// <summary>
    /// Causes the dino to 'blink' at the end of every frame,
    /// this allows us to refresh our predators in range list
    /// each frame so we can update their positions 
    /// and other variables related to that predator
    /// </summary>
    void blink()
    {
        predatorsInRange.Clear();
        friendsInRange.Clear();
        dinosInVision.Clear();
        preyInRange.Clear();
        averageTargetPos = Vector3.zero;

        //Ensure a dino cannot be seen in stereo and mono vision
        //if dino is in stereo vision
        foreach (Transform sDino in eyes.stereoVisibleTargets)
        {
            dinosInVision.Add(sDino);
        }


        //if dino is in mono vision
        foreach (Transform mDino in eyes.visibleTargets)
        {
            dinosInVision.Add(mDino);
        }


        //Check the dinos FOV for predators and store them in a list 
        foreach (Transform o in dinosInVision)
        {
            //add to our predator list
            foreach (string pTag in predators)
            {
                if (o.gameObject.CompareTag(pTag))
                {
                    predatorsInRange.Add(o);
                }
            }

            //Add to our friends list
            foreach (string fTag in friends)
            {
                if (o.gameObject.CompareTag(fTag))
                {
                    friendsInRange.Add(o);
                }
            }

            //Add to our friends list
            foreach (string prTag in prey)
            {
                if (o.gameObject.CompareTag(prTag))
                {
                    preyInRange.Add(o);
                }
            }
        }
    }
    

    public void move(Vector3 directionVector)
    {
        directionVector *= myStats.speed * Time.deltaTime;

        transform.Translate(directionVector, Space.World);
        transform.LookAt(this.transform.position + directionVector);

    }



    public void hitTarget(float damage)
    {
        myStats.health -= damage;
    }



    /// <summary>
    /// Method that allows dino to decay based on a value passed in
    /// this is to be used and determined by the dead state in or FSM
    /// </summary>
    /// <param name="decay"></param>
    public float decay(float decay)
    {
        float energyReturn = 0;

        //if we have no energy left
        if (myStats.deathEnergy <= 0)
        {
            //Destroy(this.gameObject);
        }
        //if we have energy left
        else
        {
            myStats.deathEnergy -= decay;
            if (myStats.deathEnergy < 0)
                energyReturn = myStats.deathEnergy + decay;
            else
                energyReturn = decay;
        }

        Debug.Log("Energy: " + energyReturn);
        return energyReturn;
    }
    
}
