using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Statedino;
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

    public MyAnky myAnky;
    public bool switchState = false;
    public float gametimer;
    public int seconds = 0;
    public float health = 100, energy = 35, attack = 20;
    public Statemachine<MyAnky> Statemachine { get; set; }
    public Animator anim;
    public FieldOfView ankyView;
    public List<Transform> raptors;
    public List<Transform> anky;
    public Transform ankyPosition;
    public GameObject enemy;
    public float enemyDis;
    public GameObject Friendly;
    public float friendlyDis;
    public Transform myTrans;
    public Transform myTransAnky;
    public Flee fleeAnky;
    public Herd ankyHerd;
    public Face faceRapty;
    public Wander wander;
    public Seek ankySeek;
    public GameObject rapty = null;
    public GameObject Water;
    public bool drink = false;
    // Use this for initialization
    protected override void Start()
    {

        myTrans = this.transform;
        myTransAnky = this.transform;
        myAnky = GetComponent<MyAnky>();
        ankyView = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();
        fleeAnky = GetComponent<Flee>();
        ankyHerd = GetComponent<Herd>();
        wander = GetComponent<Wander>();
        faceRapty = GetComponent<Face>();
        ankyPosition = GetComponent<Transform>();
        ankySeek = GetComponent<Seek>();
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
        StartCoroutine(energyLow());
        

        base.Start();
        //Statemachine = new Statemachine<MyAnky>(myAnky);
        
        Statemachine = new Statemachine<MyAnky>(this);

        //Statemachine.ChangeState(IdleState.instance);
        Statemachine.ChangeState(GrazingState.instance);
        fleeAnky.enabled = false;
        gametimer = Time.time;

    }


    public void FOV()
    {

        float distance = -1;
        raptors = new List<Transform>();
        float closestDistance = 10000;
        int closestIndex = 0;
        GameObject closestObject = null;

        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            if (ankyView.visibleTargets[i].gameObject.tag == "Rapty")
            {
                raptors.Add(ankyView.visibleTargets[i]);
            }
        }
        for (int i = 0; i < ankyView.stereoVisibleTargets.Count; i++)
        {
            if (ankyView.stereoVisibleTargets[i].gameObject.tag == "Rapty")
            {
                raptors.Add(ankyView.stereoVisibleTargets[i]);
            }
        }
        if (raptors.Count > 0)
        {
            for (int i = 0;  i < raptors.Count; i++)
            {
                distance = Vector3.Distance(myTrans.position, raptors[i].position);

                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                    closestObject = raptors[closestIndex].gameObject;
                }
            }
            enemy = closestObject;
            enemyDis = distance;

        }
        else
        {
            enemy = null;
            enemyDis = -1;
        }


    }

    public void Herd()
    {
        float distanceAnky = -1;
        anky = new List<Transform>();
        float closestDistanceAnky = 1;
        int closestIndexAnky = 0;
        GameObject closestObjectAnky = null;

        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            if (ankyView.visibleTargets[i].gameObject.tag == "Anky")
            {
                anky.Add(ankyView.visibleTargets[i]);
            }
        }
        for (int i = 0; i < ankyView.stereoVisibleTargets.Count; i++)
        {
            if (ankyView.stereoVisibleTargets[i].gameObject.tag == "Anky")
            {
                anky.Add(ankyView.stereoVisibleTargets[i]);
            }
        }
        if (anky.Count > 0)
        {
            for (int i = 0; i < anky.Count; i++)
            {
                distanceAnky = Vector3.Distance(myTransAnky.position, anky[i].position);

                if (distanceAnky > closestDistanceAnky)
                {
                    closestDistanceAnky = distanceAnky;
                    closestIndexAnky = i;
                    closestObjectAnky = anky[closestIndexAnky].gameObject;
                }
            }
            Friendly = closestObjectAnky;
            friendlyDis = distanceAnky;

        }
        else
        {
            Friendly = null;
            
        }
    }
    IEnumerator energyLow()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            if (energy > 0)
            {
                energy = energy - 1;
            }
        }
    }


    IEnumerator Drink()
    {
        yield return new WaitForSeconds(5.0f);
        energy = 35;
        drink = false;
        Statemachine.ChangeState(IdleState.instance);

    }
    public void Drinkypo()
    {
        StartCoroutine(Drink());
    }



    protected override void Update()
    {
        Herd();
        FOV();
        Statemachine.Update();
        

        
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here
        
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
        

        
    }

    public Vector3 distance = new Vector3();

   
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
