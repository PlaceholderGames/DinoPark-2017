using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateStuff;


public class MyAnky : Agent
{
    
    public Agent ankyAgent;
    public FieldOfView ankyView;
    public Flee ankyFlee;
    public Wander ankyWander;
    public Seek ankySeek;
    public List<Transform> ankyFriendliesClose = new List<Transform>();
    public List<Transform> ankyFriendliesFar = new List<Transform>();
    public List<Transform> ankyEnemies = new List<Transform>();
    public StateMachine<MyAnky> stateMachine { get; set; }
    public Transform target;
    public int fleeingIndex = 0;
    
    public int health = 52;
    public int anky;
    public float closestDist = 9999;
    public enum ankyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on y value of the object to denote grass level
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        GRAZING,    // Moving with the intent to find food (will happen after a random period)
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        HERDING,
        DEAD
    };

    public Animator anim;
    public ankyState currentState;

    // Use this for initialization
    protected override void Start()
    {
        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(WanderingState.Instance);
        currentState = ankyState.IDLE;
        ankyView = GetComponent<FieldOfView>();
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

    }

    protected override void Update()
    {
        // Idle - should only be used at startup
        stateMachine.Update();
        // Eating - requires a box collision with a dead dino

        // Alerted - up to the student what you do here

        // Drinking - requires y value to be below 32 (?)

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here


        //// aligning ToDo: figure out how to get it to to find the nearest anky in its circle and then move to a distance close by. 
        //for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        //{
        //    Transform target = ankyView.visibleTargets[i];
        //    if (target.tag == "Anky")
        //    {
        //        ankyFriendlies.Add(target);
        //    }
        //}
        //for (int i = 0; i < ankyFriendlies.Count; i++)
        //{
        //    Transform target = ankyFriendlies[i];
        //    float dist = Vector3.Distance(target.position, this.transform.position);

        //    if (closestDist > dist)
        //    {
        //        closestDist = dist;
        //    }
        //    //currentState = ankyState.ALIGNING;
        //    //anim.SetBool("isAlerted", false);
        //    //anim.SetBool("isDrinking", false);
        //    //anim.SetBool("isGrazing", false);
        //    //anim.SetBool("isIdle", false);
        //    //anim.SetBool("isEating", false);
        //    //anim.SetBool("isAttacking", false);
        //    //anim.SetBool("isFleeing", false);
        //    //anim.SetBool("isDead", false);
        //    //anim.SetBool("isAligning", true);
        //    ////ankyAlign.enabled = true;
        //    //ankyWander.enabled = false;
        //    //ankyAlign.target = ankyFriendlies[i].gameObject;
        //}

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    IEnumerator myCoroutine()
    {
        Debug.Log("its getting in this myCoroutine");
        yield return new WaitForSeconds(20);
    }
}
