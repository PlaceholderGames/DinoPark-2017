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

    ankyState currentState;
    ankyState previousState;

    private float hunger = 3.0f;
    private float GrazingTime = 5.0f;
    public float health = 100;
    private float thirst = 100;
    private FieldOfView sight;
   
    public float RaptyDist;
    private Transform RaptyInSight;
    

    public float raptyRange;

    public string[] viewRapty = { "Rapty" };
    public string[] viewAnky = { "Anky" };
    [Range(0, 100)]

    // public List<Transform> RaptyRange;
    //  public List<Transform> AnkyRange;
    public float fleeDistance = 50;
    public float safeTime = 4;
    private float Run = 0;


    //public Transform closestHazard = null;



    private Wander ankyWander;
    private Flee ankyFlee;
    private Face ankyFace;


    // Use this for initialization
    protected override void Start()
    {
        ankyWander = GetComponent<Wander>();


        ankyFlee = GetComponent<Flee>();

        anim = GetComponent<Animator>();
        

        sight = GetComponent<FieldOfView>();

        ankyFace = GetComponent<Face>();

        //RaptyRange = new List<Transform>();
       // AnkyRange = new List<Transform>();

        currentState = ankyState.GRAZING;


        // Assert default animation booleans and floats
        anim.SetBool("isIdle", false);
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
        base.Start();

    }

    protected override void Update()
    {
        // Idle - should only be used at startup
        Scan();
        // Eating - requires a box collision with a dead dino
        if (currentState == ankyState.EATING)
        {
            Eating();
        }
        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here
        if (currentState == ankyState.ALERTED)
        {

            AnkyAlert();
        }
        // Hunting - up to the student what you do here
        if (currentState == ankyState.GRAZING)
        {
            
            Grazing();
        }
        // Fleeing - up to the student what you do here
        if (currentState == ankyState.FLEEING)
        {
            AnkyFlee();
        }
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }


    void setState()
    {
        switch (currentState)
        {
            case ankyState.EATING:
                anim.SetBool("isEating", true);
                break;


            case ankyState.DRINKING:
                anim.SetBool("isDrinking", true);
                break;


            case ankyState.ALERTED:
                anim.SetBool("isAlerted", true);
                break;


            case ankyState.GRAZING:
                anim.SetBool("isGrazing", true);
                break;

            case ankyState.ATTACKING:
                anim.SetBool("isAttacking", true);
                break;

            case ankyState.FLEEING:
                anim.SetBool("isFleeing", true);
                break;

            case ankyState.DEAD:
                anim.SetBool("isDead", true);
                break;

        }

        switch (previousState)
        {

            case ankyState.IDLE:
                anim.SetBool("isIdle", false);
                break;

            case ankyState.EATING:
                anim.SetBool("isEating", false);
                break;


            case ankyState.DRINKING:
                anim.SetBool("isDrinking", false);
                break;


            case ankyState.ALERTED:
                anim.SetBool("isAlerted", false);
                break;


            case ankyState.GRAZING:
                anim.SetBool("isGrazing", false);
                break;

            case ankyState.ATTACKING:
                anim.SetBool("isAttacking", false);
                break;

            case ankyState.FLEEING:
                anim.SetBool("isFleeing", false);
                break;

            case ankyState.DEAD:
                anim.SetBool("isDead", false);
                break;





        }
    }







    void Eating()
    {
        hunger -= Time.deltaTime;
        ankyWander.enabled = false;

        if (hunger <= 0)
        {
            currentState = ankyState.GRAZING;
            hunger = 3.0f;
        }

    }


    void Grazing()
    {

        GrazingTime -= Time.deltaTime;
        ankyWander.enabled = true;

        if (GrazingTime <= 0)
        {
            currentState = ankyState.EATING;
            GrazingTime = 5.0f;
        }

    }

    void Scan()
    {
        for (int i = 0; i < sight.visibleTargets.Count; i++)
        {
            RaptyInSight = sight.visibleTargets[i];
            RaptyDist = Vector3.Distance(RaptyInSight.position, this.transform.position);
            Debug.Log("rapty in sight ");
            if (sight.visibleTargets[i].transform.name == "Rapty" && RaptyDist <= 150 && RaptyDist > 40)
            {
                RaptyInSight = sight.visibleTargets[i];
                //setState(ankyState.ALERTED);
                currentState = ankyState.ALERTED;

            }
            else if (sight.visibleTargets[i].tag == "Rapty" && RaptyDist <= 40)
            {
                RaptyInSight = sight.visibleTargets[i];
                // setState(ankyState.FLEEING);
                currentState = ankyState.FLEEING;
            }
        }
    }


    void AnkyAlert()
    {
        Face.RaptyInSight = RaptyInSight;
        if(!ankyFace.enabled)
        ankyFace.enabled = true;

    }

    void AnkyFlee()
    {

        Flee.RaptyInSight = RaptyInSight;
        if (!ankyFlee.enabled)
            ankyFlee.enabled = true;


    }
}