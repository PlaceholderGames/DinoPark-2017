using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateMachine;

[System.Serializable]
public struct dinoStats
{
    public int speed;
    public float health;
    public float turnSpeed;
    public float hunger;
    public float thirst;
    public float energy;
    public float deathEnergy;
}

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
        DEAD
    };


    [Header("15017931")]
    public ankyState currentState = ankyState.IDLE;
    public Agent myAgent;
    public dinoStats myStats;
    public Animator anim;
    public GameObject myTarget;
    public GameObject aStarTarget;
    [Space(10)]

    // list of objects we want to stay away from
    //contains the tags of objects that we will check are in our FOV
    [Header("Friend/Foe Tag Lists")]
	public string[] predators = {"Rapty"};
	public string[] friends = {"Anky"};
    [Space(10)]


    [Header("Vision")]
    public List<Transform> dinosInVision;
    public List<Transform> predatorsInRange;
	public List<Transform> friendsInRange;
    public float distance = 0;
    public float closestHazardDist = 100;
    public Transform closestHazard = null;
    public float herdingRange = 20;
    [Range(0, 100)]
    public float fleeDistance = 45;
    private FieldOfView eyes;
    public Vector3 averageTargetPos = Vector3.zero;
    [Space(10)]

    //Behaviour scripts
    [Header("Behaviour Scripts")]
    public Flee fleeBehaviourScript;
	public Wander wanderBehaviourScript;
    public Face faceBehaviourScript;
    public Seek seekBehaviourScript;
    public AStarSearch aStarScript;
    public ASPathFollower pathFollowerScript;
    [Space(10)]

    //If we are attacked 
    [Header("Other")]
    [Header("What We Gain")]
    public int drink = 5;
    public int eat = 5;
    [Header("What We Lose")]
    public float energyLossPerSecond = 0.5f;
    public float healthLossPerSecond = 0.5f;
    public float hungerLossPerSecond = 0.5f;
    public float thirstLossPerSecond = 0.5f;
    [Space(10)]



    [Header("Combat")]
    public int attackRange = 8;

    [Header("if We are Hit")]
    public int headDamage = 10;
    public int bodyDamage = 30;
    public int tailDamage = 10;

    [Header("if We Hit")]
    public int tailImpact = 20;
    public int headImpact = 15;
    [Space(10)]


    [Header("Death")]
    public int decayAmmount = 5;



    public StateMachine<MyAnky> stateMachine { get; set; }


    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
		fleeBehaviourScript = GetComponent<Flee> ();
		wanderBehaviourScript = GetComponent<Wander> ();
        faceBehaviourScript = GetComponent<Face>();
        aStarScript = GetComponent<AStarSearch>();
        pathFollowerScript = GetComponent<ASPathFollower>();
        seekBehaviourScript = GetComponent<Seek>();
        aStarScript = GetComponent<AStarSearch>();
        myAgent = GetComponent<Agent>();

        currentState = ankyState.IDLE;

		//Get our field of view script
		eyes = GetComponent<FieldOfView> ();
        dinosInVision = new List<Transform>();
        predatorsInRange = new List<Transform> ();
		friendsInRange = new List<Transform> ();

        myTarget = new GameObject("MyTarget");
        myTarget.transform.SetParent(this.transform);
        //myTarget = Instantiate(myTarget, this.transform);
        aStarTarget = new GameObject("AstarTarget");
        aStarScript.enabled = true;

     
        aStarScript.target = aStarTarget;
        aStarScript.target = this.gameObject;

        //set dino stats
        myStats.speed = 2;
        myStats.turnSpeed = 5;
        myStats.health = 100;
        myStats.hunger = 100;
        myStats.thirst = 100;
        myStats.energy = 100;
        myStats.deathEnergy = 100;

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

        faceBehaviourScript.enabled = false;
		fleeBehaviourScript.enabled = false;
		wanderBehaviourScript.enabled = true;



        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(IdleState.Instance);

        base.Start();

    }

    protected override void Update()
    { 
        stateMachine.Update();

        updateStats();

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
    /// Method that updates the dinos stats every second
    /// this is used to drain hunger, thrist, health
    /// </summary>
    void updateStats()
    {

        //convert hunger to energy 
        if (currentState != ankyState.DEAD)
        {
            if (myStats.hunger > 0 && myStats.energy <= 100)
            {
                myStats.hunger -= hungerLossPerSecond * Time.deltaTime;
                myStats.energy += hungerLossPerSecond * Time.deltaTime;
            }

            //convert thrist to energy
            if (myStats.thirst > 0 && myStats.energy <= 100)
            {
                myStats.thirst -= thirstLossPerSecond * Time.deltaTime;
                myStats.energy += thirstLossPerSecond * Time.deltaTime;
            }


            //if we have energy, convert this to health
            if (myStats.energy > 30 && myStats.health < 100)
            {
                myStats.energy -= energyLossPerSecond * Time.deltaTime;
                myStats.health += energyLossPerSecond * Time.deltaTime;
            }

            //if we have no energy, depleat life
            if (myStats.energy <= 0)
            {
                myStats.energy = 0;

                //Lose health over time
                myStats.health -= healthLossPerSecond * Time.deltaTime;
            }
            //lose energy over time
            else
            {
                myStats.energy -= energyLossPerSecond * Time.deltaTime;
            }

            //ensure we cant go over 100 
            if (myStats.energy >= 100)
                myStats.energy = 100;
            if (myStats.health >= 100)
                myStats.health = 100;
            if (myStats.thirst >= 100)
                myStats.thirst = 100;
            if (myStats.hunger >= 100)
                myStats.hunger = 100;
        }
        else
        {
            myStats.energy = 0;
            myStats.health = 0;
            myStats.thirst = 0;
            myStats.hunger = 0;
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
		predatorsInRange.Clear ();
        friendsInRange.Clear();
        dinosInVision.Clear();
        averageTargetPos = Vector3.zero;

        closestHazardDist = 100;
        closestHazard = null;


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
                    if (o.GetComponent<MyRapty>().currentState != MyRapty.raptorState.DEAD)
                    {
                        predatorsInRange.Add(o);
                        averageTargetPos += (o.position);
                    }
                }
            }

            //Add to our friends list
            foreach (string fTag in friends)
            {
                if (o.gameObject.CompareTag(fTag))
                {
                    if (o.GetComponent<MyAnky>().currentState != MyAnky.ankyState.DEAD)
                    {
                        friendsInRange.Add(o);
                    }
                }
            }
        }


        if (predatorsInRange.Count > 0)
        {
            averageTargetPos = new Vector3(averageTargetPos.x / predatorsInRange.Count, averageTargetPos.y / predatorsInRange.Count, averageTargetPos.z / predatorsInRange.Count);
            myTarget.transform.position = averageTargetPos;
        }
    }

	/// <summary>
	/// update our FSM Bools based on our current state
	/// </summary>
	void checkStatus()
	{
		Debug.Log (currentState);
	}

    public void setCurrentState(ankyState newState)
    {
        currentState = newState;
    }


   public void move(Vector3 directionVector)
    {
        directionVector *= myStats.speed * Time.deltaTime;

        transform.Translate(directionVector, Space.World);
        transform.LookAt(this.transform.position + directionVector);

    }


    /// <summary>
    /// Method that finds friends and encorages us to move towards their average position
    /// </summary>
    public void herd()
    {
        Vector3 averagePos = Vector3.zero;
        int average;
        //if we have friends in range
        if(friendsInRange.Count > 0 )
        {
            average = friendsInRange.Count + 1;

            foreach(Transform friend in friendsInRange)
            {
                averagePos += friend.position;
            }
            averagePos += this.transform.position;
            //set our average position for friends 
            averagePos = new Vector3(averagePos.x/ average, averagePos.y /average , averagePos.z / average);           
            
            //Update our target position to the averge position
            myTarget.transform.position = averagePos;

            Debug.DrawLine(this.transform.position, myTarget.transform.position);

            //Check if we are outside our herding range
            if (Vector3.Distance(this.transform.position, averagePos) > herdingRange)
            {
                wanderBehaviourScript.enabled = false;

                //move closer to the average position
                Vector3 targetDir = averagePos - myAgent.transform.position;
      
                seekBehaviourScript.target = myTarget;
                seekBehaviourScript.enabled = true;
            }
            else
            {
                wanderBehaviourScript.enabled = true;
            }
        }
        else
        {
            seekBehaviourScript.enabled = false;
        }

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (myTarget != null)
            Gizmos.DrawWireSphere(myTarget.transform.position, herdingRange);
       

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
            Destroy(this.gameObject);
        }
        //if we have energy left
        else
        {
            myStats.deathEnergy -= decay;
            if (myStats.deathEnergy  < 0)
                energyReturn = myStats.deathEnergy + decay;
            else
                energyReturn = decay;
        }

        Debug.Log("Energy: " + energyReturn);
        return energyReturn;
    }
}
