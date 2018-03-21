using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateMachine;

[System.Serializable]
public struct dinoStats
{
    public int health;
    public int hunger;
    public int thirst;
    public int energy;
    public int deathEnergy;
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
    public dinoStats myStats;
    public Animator anim;
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
    [Space(10)]

    //If we are attacked 
    [Header("Other")]
    [Header("What We Gain")]
    public int drink = 5;
    public int eat = 5;
    [Header("What We Lose")]
    public int energyOverTime = 5;
    [Space(10)]



    [Header("Combat")]
    public int attackRange = 10;

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

		currentState = ankyState.IDLE;

		//Get our field of view script
		eyes = GetComponent<FieldOfView> ();
        dinosInVision = new List<Transform>();
        predatorsInRange = new List<Transform> ();
		friendsInRange = new List<Transform> ();


        //set dino stats
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

		fleeBehaviourScript.enabled = false;
		wanderBehaviourScript.enabled = true;



        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(IdleState.Instance);

        base.Start();

    }

    protected override void Update()
    {

        stateMachine.Update();
        Debug.DrawLine(this.transform.position, averageTargetPos);

        base.Update();
    }

    /// <summary>
    /// Check latest positions after each physics update
    /// </summary>
    protected override void LateUpdate()
    {
        blink();
        //checkStatus ();
        base.LateUpdate();
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
				if (o.gameObject.CompareTag (pTag)) 
				{
					predatorsInRange.Add (o);
                    averageTargetPos += (o.position);
				}


			}

			//Add to our friends list
			foreach (string fTag in friends) 
			{
				if (o.gameObject.CompareTag (fTag)) 
				{
					friendsInRange.Add (o);
				}
			}
		}

        if (predatorsInRange.Count > 0)
        {
            averageTargetPos = new Vector3(averageTargetPos.x / predatorsInRange.Count, averageTargetPos.y / predatorsInRange.Count, averageTargetPos.z / predatorsInRange.Count);
            
        }
        else
        {
            faceBehaviourScript.enabled = false;
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


    /// <summary>
    /// Method that allows dino to decay based on a value passed in
    /// this is to be used and determined by the dead state in or FSM
    /// </summary>
    /// <param name="decay"></param>
    public int decay(int decay)
    {
        int energyReturn = 0;

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
