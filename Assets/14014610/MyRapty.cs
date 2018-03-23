using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Statestuff;

public class MyRapty : Agent
{

    public bool switchState = false;

    public StateMachine<MyRapty> stateMachine { get; set; }

    public List<Transform> enemies = new List<Transform>();
    public List<Transform> friendlies = new List<Transform>();
    public FieldOfView fov;
    public Wander raptyWander;
    public Seek raptySeek;
    public Face raptyFace;

    public GameObject water;

    public double hydration = 100;
    public double energy = 100;
    public double health = 100;
    public enum raptyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on location of a target object (killed prey)
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        HUNTING,    // Moving with the intent to hunt
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };
    public Animator anim;
    public raptyState currentraptyState;

    // Use this for initialization
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller

        stateMachine = new StateMachine<MyRapty>(this);
        stateMachine.ChangeState(raptyHuntingState.Instance);

        base.Start();
    }

    protected override void Update()
    {
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed


        enemies.Clear();
        foreach (Transform target in fov.visibleTargets)
        {
            if (target.tag == "Anky" && !enemies.Contains(target))
            {
                enemies.Add(target);
            }
        }
        foreach (Transform target in fov.stereoVisibleTargets)
        {
            if (target.tag == "Anky" && !enemies.Contains(target))
            {
                enemies.Add(target);
            }
        }
        friendlies.Clear();
        foreach (Transform target in fov.visibleTargets)
        {
            if (target.tag == "Rapty" && !friendlies.Contains(target))
            {
                if (target.transform.position != transform.position)
                    friendlies.Add(target);
            }
        }
        foreach (Transform target in fov.stereoVisibleTargets)
        {
            if (target.tag == "Rapty" && !friendlies.Contains(target))
            {
                if (target.transform.position != transform.position)
                    friendlies.Add(target);
            }
        }



        if (hydration > 0)
            hydration -= (Time.deltaTime * 0.2) * 1;
        else
            health -= (Time.deltaTime * 0.2) * 1;

        if (energy > 0)
            energy -= (Time.deltaTime * 0.15) * 1;
        else
            health -= (Time.deltaTime * 0.2) * 1;

        foreach (Transform enemy in enemies)
        {
            float enemyDist = Vector3.Distance(enemy.position, transform.position);
            if (enemyDist < 5)
            {
                health -= (Time.deltaTime * 1) * 1;
            }
        }

        stateMachine.Update();

        base.Update();
    }

    private void OnCollisionEnter(Collision Col)
    {
        if (Col.gameObject.tag == "Anky")
            health -= 5;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
