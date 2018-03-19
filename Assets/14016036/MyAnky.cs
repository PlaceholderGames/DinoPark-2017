using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAnky : Agent
{
    
    public Agent ankyAgent;
    public FieldOfView ankyView;
    public Flee ankyFlee;
    public Wander ankyWander;
    public DrinkingS ankyDrinkingS;
    public List<Transform> ankyFriendlies = new List<Transform>();
    public int health = 52;
    int anky;
    float closestDist = 9999;
    public enum ankyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on y value of the object to denote grass level
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        GRAZING,    // Moving with the intent to find food (will happen after a random period)
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        ALIGNING,
        DEAD
    };

    public Animator anim;
    ankyState currentState;

    // Use this for initialization
    protected override void Start()
    {
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

        // Eating - requires a box collision with a dead dino

        // Alerted - up to the student what you do here
        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            Transform target = ankyView.visibleTargets[i];
            if (ankyView.visibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, this.transform.position) <= 150)
            {
                anky = 1;
                currentState = ankyState.ALERTED;

                anim.SetTrigger("isAlert");
                anim.SetBool("isAlerted", true);
                anim.SetBool("isDrinking", false);
                anim.SetBool("isGrazing", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isEating", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isFleeing", false);
                anim.SetBool("isDead", false);
                Debug.Log("its getting in this Alert");

            }
        }
        if (anky <= 0)
        {
            anim.SetBool("isAlerted", false);
            anim.SetBool("isDrinking", false);
            anim.SetBool("isGrazing", true);
            anim.SetBool("isIdle", false);
            anim.SetBool("isEating", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isFleeing", false);
            anim.SetBool("isDead", false);
        }
        anky = 0;
        // Drinking - requires y value to be below 32 (?)

        if (this.transform.position.y < 35.5 && anim.GetBool("isGrazing") == true || this.transform.position.y < 35.5 && anim.GetBool("isAlerted") == true)
        {
            currentState = ankyState.DRINKING;
            drinking();
        }
        anky = 0;
        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here
        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            Transform target = ankyView.visibleTargets[i];
            if (ankyView.visibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, this.transform.position) < 40 && anim.GetBool("isAlerted") == true || ankyView.visibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, this.transform.position) < 40 && anim.GetBool("isAttacking") == true && health < 20)
            {
                currentState = ankyState.FLEEING;
                anim.SetTrigger("isFleeing");
                anim.SetBool("isAlerted", false);
                anim.SetBool("isDrinking", false);
                anim.SetBool("isGrazing", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isEating", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isFleeing", true);
                anim.SetBool("isDead", false);
                Debug.Log("its getting in this Alert");
                ankyFlee.enabled = true;
                ankyWander.enabled = false;
                ankyAgent.maxSpeed = 3;
                ankyFlee.target = ankyView.visibleTargets[i].gameObject;
            }
            else if (ankyView.visibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, this.transform.position) > 50 && Vector3.Distance(target.position, this.transform.position) <= 150)
            {
                currentState = ankyState.ALERTED;
                anim.SetBool("isAlerted", true);
                anim.SetBool("isDrinking", false);
                anim.SetBool("isGrazing", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isEating", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isFleeing", false);
                anim.SetBool("isDead", false);
                ankyWander.enabled = true;
                ankyFlee.enabled = false;
                ankyAgent.maxSpeed = 1;
            }
        }

        // aligning ToDo: figure out how to get it to to find the nearest anky in its circle and then move to a distance close by. 
        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            Transform target = ankyView.visibleTargets[i];
            if (target.tag == "Anky")
            {
                ankyFriendlies.Add(target);
            }
        }
        for (int i = 0; i < ankyFriendlies.Count; i++)
        {
            Transform target = ankyFriendlies[i];
            float dist = Vector3.Distance(target.position, this.transform.position);
            
            if (closestDist > dist)
            {
                closestDist = dist;
            }
            //currentState = ankyState.ALIGNING;
            //anim.SetBool("isAlerted", false);
            //anim.SetBool("isDrinking", false);
            //anim.SetBool("isGrazing", false);
            //anim.SetBool("isIdle", false);
            //anim.SetBool("isEating", false);
            //anim.SetBool("isAttacking", false);
            //anim.SetBool("isFleeing", false);
            //anim.SetBool("isDead", false);
            //anim.SetBool("isAligning", true);
            ////ankyAlign.enabled = true;
            //ankyWander.enabled = false;
            //ankyAlign.target = ankyFriendlies[i].gameObject;
        }

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
    public void drinking()
    {
        //AnimatorStateInfo ankyStateInfo;
        //currentState = ankyState.DRINKING;
        //currentState = ankyState.DRINKING;
        if (anim.GetBool("isGrazing") == true)
        {
            anky = 1;
        }
        anim.SetBool("isDrinking", true);
        anim.SetTrigger("isDrinking");
        if (anim.GetBool("isDrinking") == true)
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isEating", false);
            anim.SetBool("isAlerted", false);
            anim.SetBool("isGrazing", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isFleeing", false);
            anim.SetBool("isDead", false);
            Debug.Log("its getting in this Drink");
            //StartCoroutine(myCoroutine());
            //health = 100;
        }
        else
        {
            if (anky == 1)
            {
                anim.SetBool("isGrazing", true);
                currentState = ankyState.GRAZING;
            }
            else
            {
                anim.SetBool("isAlerted", true);
                currentState = ankyState.ALERTED;
            }
            anim.SetBool("isDrinking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isEating", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isFleeing", false);
            anim.SetBool("isDead", false);
        }
    }
}
