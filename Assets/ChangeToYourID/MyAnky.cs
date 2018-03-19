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
	public Flee run;
    private FieldOfView fov;
	private Transform player = null;
	private Transform ankyT = null;
	public float fleeDistance = 5.0f;
    GameObject[] raptors = null;

    // Use this for initialization
    protected override void Start()
    {
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

		GameObject obj = GameObject.FindWithTag("Player");
		player = obj.transform;
		ankyT = transform;
		run = gameObject.GetComponent<Flee> ();
        fov = gameObject.GetComponent<FieldOfView>();
    }

    protected override void Update()
    {
        for (int i = 0; i < fov.visibleTargets.Count; i++)
        {
            if (fov.visibleTargets)
        }

        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here
		float dist = (player.position- ankyT.position).magnitude;
		if (dist > fleeDistance) 
			run.isFlee();
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
