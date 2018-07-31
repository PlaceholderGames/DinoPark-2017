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
        DEAD		// Zero hit points, food points or water points.
    };

	const int maximum = 100000;					//Maximum capacity for food and water.
	const float foodWaterThreshold = 40.0f;		//The terrain height that differentiates between eating and drinking.

    public Animator anim;
	[SerializeField]	//Required to see the next variable in the inspector, as it is a private variable.
    private int health = 100;     	//Current percentage health of this anky.
	[SerializeField]	//Required to see the next variable in the inspector, as it is a private variable.
	private int foodLevels = maximum;	//Current food level of this anky.
	[SerializeField]	//Required to see the next variable in the inspector, as it is a private variable.
	private int waterLevels = maximum;	//Current water level of this anky.
	[SerializeField]	//Required to see the next variable in the inspector, as it is a private variable.
	private float currentTerrainHeight = 0.0f;      //Height of the current terrain level at the point where this anky is.
	[SerializeField]	//Required to see the next variable in the inspector, as it is a private variable.
	private int fearLevels = 0;
    private bool startup = true;    //Startup check. Used for idle state.
	private int dinoListSize = 0;
	private bool IamHungry = false;
	private bool IamThirsty = false;
	private bool alive = true;
	private GameObject Target = null;
    ankyState current;   //Create a variable representing the existing state.

    //Initialise scripts
    public Flee fleeBehaviourScript;
    public Wander wanderBehaviourScript;
    public Face faceBehaviourScript;
	public Seek seekBehaviourScript;

    // Use this for initialization
    protected override void Start()
    {
        /* Animations disabled due to incorrect implementation.
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
		*/

        current = ankyState.IDLE;   //Initialise.

        //Link scripts together
        fleeBehaviourScript = GetComponent<Flee>();
        wanderBehaviourScript = GetComponent<Wander>();
        faceBehaviourScript = GetComponent<Face>();
		seekBehaviourScript = GetComponent<Seek>();

		foodLevels = maximum;	//Initialise
		waterLevels = maximum;	//Initialise
		currentTerrainHeight = 0.0f; 				//Initialise
    }

    protected override void Update()
    {
        // Idle - should only be used at startup
        if (startup == true)
        {
            startup = false;
            current = ankyState.IDLE;
            Idle();
        }
		if (alive == true) 
		{
			if (fearLevels > 0) {fearLevels -= 1;}	//Loss of fear per update.
			foodLevels -= 2;		//Loss of food per update.
			waterLevels -= 3;		//Dehydration worse than starvation.
			currentTerrainHeight = this.transform.position.y;	//Get the current dino's terrain height.

			//Debug.Log("height: " + y);	//Check height values
			//Debug.Log("Food: " + foodLevels);	//Check food values
			//Debug.Log("Water: " + waterLevels);	//Check water values

			if (foodLevels < maximum * 0.75)
			{
				IamHungry = true;
			}

			if (waterLevels < maximum * 0.75)
			{
				IamThirsty = true;
			}

			// Grazing - wander around as per description. Default activity for anky dinosaurs.
			if (fearLevels <= 0)
			{
				current = ankyState.GRAZING;
				Grazing ();
			}



			// Eating - requires current terrain height to be above 40, which is the food/water threshold.
			if (currentTerrainHeight > foodWaterThreshold && IamHungry == true && fearLevels <= 0)
			{
				if (foodLevels >= maximum)
				{
					IamHungry = false;
				}
				foodLevels += 6;		//Eating recovers faster than starving.
				current = ankyState.EATING;
				Eating ();
			}

			// Drinking - requires current terrain height to be 40 or less.
			if (currentTerrainHeight <= foodWaterThreshold && IamThirsty == true && fearLevels <= 0)
			{
				if (waterLevels >= maximum)
				{
					IamThirsty = false;
				}
				waterLevels += 13;		//Drinking is faster than eating.
				current = ankyState.DRINKING;
				Drinking ();
			}
			

			// Alerted - Unused, as fleeing covers this anyway.
			if (false)
			{
				current = ankyState.ALERTED;
				Alerted ();
			}
			

			// Fleeing - run from raptors
			FieldOfView fov = GetComponent<FieldOfView> ();
			dinoListSize = fov.visibleTargets.Count;
			//Debug.Log(dinoListSize);	//Check list size.

			for (int i = 0; i < dinoListSize; i++)
			{
				//Debug.Log("Check for raptors");
				if (fov.visibleTargets [i].tag == "Rapty")
				{
					//Debug.Log("Flee");
					fearLevels = 3000;
					IamHungry = false;
					IamThirsty = false;
					current = ankyState.FLEEING;
					Target = fov.visibleTargets [i].gameObject;
					Fleeing ();
					break;
				}
			}
			

			// Dead - Cease all functions and become a corpse.
			if (health <= 0 || foodLevels <= 0 || waterLevels <= 0)
			{
				current = ankyState.DEAD;
				alive = false;
				foodLevels = 0;
				waterLevels = 0;
				health = 0;
				fearLevels = 0;
				Dead ();
			}

			base.Update ();
		}
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    //Functions for the states that are going to be used.
    // Idle - should only be used at startup
    void Idle()
    {
		//Animations do not work properly, so they are not currently in use.
        //anim.SetBool("isIdle", true);

		//Debug.Log("idle");
        fleeBehaviourScript.enabled = false;
        wanderBehaviourScript.enabled = false;
		seekBehaviourScript.enabled = false;
        //faceBehaviourScript.enabled = false;
    }


    // Eating - requires y value to be above 40.
    void Eating()
    {
		//Animations do not work properly, so they are not currently in use.
		/*
		anim.SetBool("isEating", true);
		anim.SetBool("isFleeing", false);
		anim.SetBool("isDrinking", false);
		anim.SetBool("isGrazing", false);
		*/

		Debug.Log("eat");
		fleeBehaviourScript.enabled = false;
		wanderBehaviourScript.enabled = false;
		seekBehaviourScript.enabled = false;
		//faceBehaviourScript.enabled = false;
    }


    // Drinking - requires y value to be below 40 or less.
    void Drinking()
    {
		//Animations do not work properly, so they are not currently in use.
		/*
        anim.SetBool("isDrinking", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isGrazing", false);
        */

		Debug.Log("drink");
        fleeBehaviourScript.enabled = false;
		seekBehaviourScript.enabled = false;
        wanderBehaviourScript.enabled = false;
        //faceBehaviourScript.enabled = false;
    }
    // Alerted - Not used
    void Alerted()
    {
		//Animations do not work properly, so they are not currently in use.
		/*
        anim.SetBool("isAlerted", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isDrinking", false);
        */

        //Run the "AgentAwared" script in Assets -> dinosaurs -> Agent -> Scripts -> AgentAwareness
        fleeBehaviourScript.enabled = false;
        wanderBehaviourScript.enabled = false;
		seekBehaviourScript.enabled = false;
        //faceBehaviourScript.enabled = true;
        //_owner.faceBehaviourScript.target = owner.myTarget;
    }
    // Grazing - up to the student what you do here
    void Grazing()  //This is searching for food according to above
    {
		//Animations do not work properly, so they are not currently in use.
		/*
        anim.SetBool("isGrazing", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDrinking", false);
        */

		if (waterLevels < maximum * 0.5) 
		{
			Debug.Log ("finding water");
			Target = GameObject.Find("RockMeshWater");
			this.seekBehaviourScript.target = this.Target;
			seekBehaviourScript.enabled = true;
			fleeBehaviourScript.enabled = false;
			wanderBehaviourScript.enabled = false;
			//faceBehaviourScript.enabled = false;
		} 
		else if (foodLevels < maximum * 0.5) 
		{
			Debug.Log ("finding food");
			Target = GameObject.Find("RockMeshFood");
			this.seekBehaviourScript.target = this.Target;
			seekBehaviourScript.enabled = true;
			fleeBehaviourScript.enabled = false;
			wanderBehaviourScript.enabled = false;
			//faceBehaviourScript.enabled = false;
		} 
		else 
		{
			//Run the "Wander" script in Assets -> dinosaurs -> Agent -> Scripts -> Behaviours
			Debug.Log("wander");
			fleeBehaviourScript.enabled = false;
			seekBehaviourScript.enabled = false;
			wanderBehaviourScript.enabled = true;
			//faceBehaviourScript.enabled = false;
		}

    }
    // Fleeing - up to the student what you do here
    void Fleeing()
    {
		//Animations do not work properly, so they are not currently in use.
		/*
        anim.SetBool("isFleeing", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isGrazing", false);
        */

		Debug.Log("run");
		this.fleeBehaviourScript.target = this.Target;
        fleeBehaviourScript.enabled = true;
        wanderBehaviourScript.enabled = false;
		seekBehaviourScript.enabled = false;
		//this.faceBehaviourScript.target = this.Target;
        //faceBehaviourScript.enabled = true;
		//Debug.Log(this.Target);
    }
    // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
    void Dead()
    {
		//Animations do not work properly, so they are not currently in use.
		/*
        anim.SetBool("isDead", true);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
		*/

	   Debug.Log("dead");
       fleeBehaviourScript.enabled = false;
       wanderBehaviourScript.enabled = false;
	   seekBehaviourScript.enabled = false;
       //faceBehaviourScript.enabled = false;
    }
}
