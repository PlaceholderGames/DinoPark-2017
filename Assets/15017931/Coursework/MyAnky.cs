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
	[Range(0,100)]
	public float fleeDistance = 45;
	//Time that dino will continue to flee when they cant see a predator
	public float fleeNoVisTime = 2;

	private FieldOfView eyes;
	private ankyState currentState = ankyState.IDLE;
	private List<Transform> predatorsInRange;

	private float running = 0;

    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
		currentState = ankyState.IDLE;

		//Get our field of view script
		eyes = GetComponent<FieldOfView> ();
		predatorsInRange = new List<Transform> ();

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
		anim.SetBool("isGrazing", true);
        // Idle - should only be used at startup
		idleBehaviour();
        // Eating - if on grass and we are hungry
		eatBehaviour();
        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here
		//If we have a predator in our vision
		//possibly if predator is within a certain distance
		//Use FOV Script 

		//check all objects in our FOV
		//Check if we are not in current state
	
		alertBehaviour ();




        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here
	
		fleeBehaviour ();		
	
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed



		//
		blink();
		checkStatus ();
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }


	/// <summary>
	/// Default Behaviour (when no paramaters are set)
	/// </summary>
	void idleBehaviour()
	{

	}


	/// <summary>
	/// Eating behaviour 
	/// If dino is on food and health is not 100%
	/// eat to regain health
	/// </summary>
	void eatBehaviour()
	{

	}


	/// <summary>
	/// Drink behaviour
	/// Will likely make use of A* to allow the dino to find
	/// local source of water to allow dino to drink
	/// </summary>
	void drinkBehaviour()
	{


	}

	/// <summary>
	/// Checks the dinos vision to see if there are any hazards approaching
	/// any objects with a given predator tag will be added to a list of visible
	/// predators, this will be used by other methods to determin behaviour
	/// </summary>
	void alertBehaviour()
	{
		//Check the dinos FOV for predators and store them in a list 
		foreach (Transform o in eyes.visibleTargets) 
		{	
			foreach (string pTag in predators) 
			{
				if (o.gameObject.CompareTag (pTag)) 
				{
					predatorsInRange.Add (o);
				}
			}
		}

		//if we have a predator in our vision, set our alerted state to true
		if (predatorsInRange.Count > 0) 
		{
			currentState = ankyState.ALERTED;
			anim.SetBool ("isAlerted", true);
			anim.SetBool ("isGrazing", false);
			anim.SetBool ("isDrinking", false);
			anim.SetBool ("isEating", false);
		} 
		else 
		{
			currentState = ankyState.ALERTED;
			anim.SetBool ("isAlerted", false);
		}
	}

	/// <summary>
	/// Method will check our predators in range list for distance to closest hazard 
	/// and cause the dino to change state depending on how close the hazard is.
	/// predators can be within a threshold before behaviour will change to fleeing
	/// </summary>
	void fleeBehaviour()
	{
		float distance;
		float closestHazardDist = 100;
		Transform closestHazard = null;

		//Cycle through our list of visible predators and find out closest hazard
		foreach (Transform i in predatorsInRange) {	
			distance = Vector3.Distance (i.transform.position, this.transform.position);
			Debug.Log (distance);

			//check for our closest predator is closer than our current known closest 
			if (distance < closestHazardDist) {
				//Debug.Log ("Found something closer");
				closestHazardDist = distance;
				closestHazard = i;
			}

			//Debug.Log ("Closest Hazard = " + closestHazard);
		}

		//If our hazard is within our flee radius
		if (closestHazardDist < fleeDistance) {
			currentState = ankyState.FLEEING;
			anim.SetBool ("isFleeing", true);
		}
	
		//If we can no longer see a predator, continue running for n.. seconds
		if (predatorsInRange.Count < 0) {
			//if we are currently fleeing 
			if (running < fleeNoVisTime) {
				running += Time.deltaTime;
			} else {
				running = 0;
				anim.SetBool ("isFleeing", false);
				currentState = ankyState.ALERTED;
			}
		} else {
			running = 0;
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
	}


	void checkStatus()
	{
		if (currentState != ankyState.IDLE) {
			anim.SetBool ("isIdle", false);
		} else {
			anim.SetBool ("isIdle", true);
		}

	}
}
