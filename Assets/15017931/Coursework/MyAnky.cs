using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public Animator anim;
	// list of objects we want to stay away from
	//contains the tags of objects that we will check are in our FOV
	public string[] predators = {"Rapty"};
	public string[] friends = {"Anky"};
	[Range(0,100)]
	public float fleeDistance = 45;
	//Time that dino will continue to flee when they cant see a predator
	public float fleeNoVisTime = 2;

	private FieldOfView eyes;
	private ankyState currentState = ankyState.IDLE;
	private List<Transform> predatorsInRange;
	private List<Transform> friendsInRange;

	//Behaviour scripts
	private Flee fleeBehaviourScript;
	private Wander wanderBehaviourScript;

	private float running = 0;

	private float distance = 0;
	float closestHazardDist = 100;
	Transform closestHazard = null;

	[Range(0,30)]
	public float energyPerBite = 10;
	private float decayAmmount = 0;

	public float health = 100;
	private float hunger = 100;
	private float thirst = 100;


    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
		fleeBehaviourScript = GetComponent<Flee> ();
		wanderBehaviourScript = GetComponent<Wander> ();

		currentState = ankyState.IDLE;

		//Get our field of view script
		eyes = GetComponent<FieldOfView> ();
		predatorsInRange = new List<Transform> ();
		friendsInRange = new List<Transform> ();

        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller

		fleeBehaviourScript.enabled = false;
		wanderBehaviourScript.enabled = true;

        base.Start();

    }

    protected override void Update()
    {

        // Idle - should only be used at startup
		if(currentState == ankyState.IDLE)
			idleBehaviour();

		//Grazing - default state when we are happy
		if (currentState == ankyState.GRAZING) {
			Debug.Log ("GRAZING");
			grazingBehaviour ();
		}
        // Eating - if on grass and we are hungry
		if (currentState == ankyState.EATING) {
			Debug.Log ("EATING");
			eatBehaviour ();
		}
		// Drinking - requires y value to be below 32 (?)
		if (currentState == ankyState.DRINKING) {
			Debug.Log ("DRINK");
			drinkBehaviour ();
		}
		// Alerted - up to the student what you do here
		if (currentState == ankyState.ALERTED) {
			Debug.Log ("ALERT");
			alertBehaviour ();
		}
        // Hunting - up to the student what you do here
		if (currentState == ankyState.ATTACKING) {
			Debug.Log ("ATTACK");
		}
			
        // Fleeing - up to the student what you do here
		if (currentState == ankyState.FLEEING) {
			Debug.Log ("FLEE");
			fleeBehaviour ();		
		}
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
		if (currentState == ankyState.DEAD) {
			Debug.Log ("DEAD");
			deadBehaviour ();
		}

		Debug.Log ("In Vision: " + predatorsInRange.Count);

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
	/// Default Behaviour (when no paramaters are set)
	/// </summary>
	void idleBehaviour()
	{
		currentState = ankyState.GRAZING;
	}

	/// <summary>
	/// Default state
	/// will wander around deciding what to do next
	/// </summary>
	void grazingBehaviour()
	{
		wanderBehaviourScript.enabled = true;

		//If we have predators in range
		//if we have a predator in our vision, set our alerted state to true
		if (predatorsInRange.Count > 0) {
			anim.SetBool ("isAlerted", true);
			anim.SetBool ("isGrazing", false);
			currentState = ankyState.ALERTED;
		} 
		//Drinking
		else if (false) {
			anim.SetBool ("isGrazing", false);
			anim.SetBool ("isDrinking", true);
		}
		//Eating
		else if (false) {
			anim.SetBool ("isGrazing", false);
			anim.SetBool ("isEating", true);
		}
		//Stay in our current state
		else {
			anim.SetBool ("isGrazing", true);		
		}

	}

	/// <summary>
	/// Eating behaviour 
	/// If dino is on food and health is not 100%
	/// eat to regain health
	/// </summary>
	void eatBehaviour()
	{
		//If there are predators near
		if (predatorsInRange.Count > 0) 
		{
			currentState = ankyState.ALERTED;
		} 
		else 
		{
			//If we are still hungry 
			if(false)
			{
				//eat - continue eating
			}
			//If not then go back to grazing
			else
			{
				currentState = ankyState.GRAZING;
			}
		}
	}


	/// <summary>
	/// Drink behaviour
	/// Will likely make use of A* to allow the dino to find
	/// local source of water to allow dino to drink
	/// </summary>
	void drinkBehaviour()
	{
		//If there are predators near
		if (predatorsInRange.Count > 0) 
		{
			currentState = ankyState.ALERTED;
		} 
		else 
		{
			//If we are still thirsty 
			if(false)
			{
				//drink - continue drinking
			}
			//If not then go back to grazing
			else
			{
				currentState = ankyState.GRAZING;
			}
		}

	}

	/// <summary>
	/// Checks the dinos vision to see if there are any hazards approaching
	/// any objects with a given predator tag will be added to a list of visible
	/// predators, this will be used by other methods to determin behaviour
	/// </summary>
	void alertBehaviour()
	{
		//Check distance between us and our closest hazard
		foreach (Transform pred in predatorsInRange) {
			//Distance between us and predator
			distance = Vector3.Distance (this.transform.position, pred.transform.position);
			//find the closets predator
			if (distance < closestHazardDist) {
				closestHazard = pred;
				closestHazardDist = distance;
			}
		}

		//if there are predators in range
		if (predatorsInRange.Count > 0) {
			//Should we flee?
			//If our hazard is within our flee radius
			if (closestHazardDist <= fleeDistance) {  // is there a predator in our safe space * 
				wanderBehaviourScript.enabled = false;
				fleeBehaviourScript.target = closestHazard.gameObject;
				fleeBehaviourScript.enabled = true;

				anim.SetBool ("isFleeing", true);
				currentState = ankyState.FLEEING;

			} else {
				wanderBehaviourScript.enabled = true;
				fleeBehaviourScript.enabled = false;
				fleeBehaviourScript.target = null;
			}
		}
		//if there are no longer predators in range
		else{
			//Should we Graze
			if (true) {
				wanderBehaviourScript.enabled = true;
				anim.SetBool ("isGrazing", true);
				currentState = ankyState.GRAZING;
			}
			//Should we Eat
			else if (false) {
				anim.SetBool ("isEating", true);
				currentState = ankyState.EATING;
			}
			//Should we Drink
			else if (false) {
				anim.SetBool ("isDrinking", true);
				currentState = ankyState.DRINKING;
			}
		}
	}

	/// <summary>
	/// Method will check our predators in range list for distance to closest hazard 
	/// and cause the dino to change state depending on how close the hazard is.
	/// predators can be within a threshold before behaviour will change to fleeing
	/// </summary>
	void fleeBehaviour(){

		//If we can no longer see a predator, continue running for n.. seconds
		if (predatorsInRange.Count <= 0) {
			//if we are currently fleeing 
			if (running < fleeNoVisTime) {
				running += Time.deltaTime;
			} else {
				anim.SetBool ("isFleeing", false);
				fleeBehaviourScript.enabled = false;
				fleeBehaviourScript.target = null;
				running = 0;
				anim.SetBool ("isAlerted", true);
				currentState = ankyState.ALERTED;

			}
		}
	}

	/// <summary>
	/// Method that controls what happens after the death of dino
	/// </summary>
	void deadBehaviour()
	{
		//If we have been eaten
		if (decayAmmount >= 100)
			GameObject.Destroy (this.gameObject);
	}


	/// <summary>
	/// Method for if our dino is dead and another dino is eating us.
	/// allows predator to replenish its energy per bite.
	/// </summary>
	/// <returns>energy</returns>
	/// <param name="biteSize">Bite size.</param>
	public float eatForEnergy(float biteSize)
	{
		decayAmmount += biteSize;

		//allow dino to take bigger bites of victim
		if (biteSize < 20)
			return 10;
		else if (biteSize < 50)
			return 20;
		else
			return 30;
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

		//Check the dinos FOV for predators and store them in a list 
		foreach (Transform o in eyes.visibleTargets) 
		{	
			//add to our predator list
			foreach (string pTag in predators) 
			{
				if (o.gameObject.CompareTag (pTag)) 
				{
					predatorsInRange.Add (o);
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
	}

	/// <summary>
	/// update our FSM Bools based on our current state
	/// </summary>
	void checkStatus()
	{
		Debug.Log (currentState);
	}
}
