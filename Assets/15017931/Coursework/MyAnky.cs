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
	public float alertRadius = 45;

	private FieldOfView eyes;
	private ankyState currentState = ankyState.IDLE;
	private List<Transform> predatorsInRange;

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

        // Eating - if on grass and we are hungry

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here
		//If we have a predator in our vision
		//possibly if predator is within a certain distance
		//Use FOV Script 

		//check all objects in our FOV
		//Check if we are not in current state
	
		foreach (Transform o in eyes.visibleTargets) {	
			//Check if any of those objects are a predator
			foreach (string pTag in predators) {
				if (o.gameObject.CompareTag (pTag)) {
					predatorsInRange.Add (o);
					//Debug.Log (o.gameObject.tag);
				}
			}
		}

	
		if (predatorsInRange.Count > 0) {
			//currentState = ankyState.ALERTED;
			anim.SetBool ("isAlerted", true);
		} else {
			//currentState = ankyState.ALERTED;
			anim.SetBool ("isAlerted", false);
		}




        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here
		if (currentState == ankyState.ALERTED) {
			//check if raptor is within our 'Worry Range'
			foreach (Transform o in eyes.visibleTargets) 
			{	
				//Check if any of those objects are a predator
				foreach (string pTag in predators) 
				{
					if (o.gameObject.CompareTag (pTag)) 
					{

					}
				}
			}
		}
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed



		//
		blink();
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

	void blink()
	{
		predatorsInRange.Clear ();
	}
}
