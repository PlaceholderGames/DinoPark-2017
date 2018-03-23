using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAnky : Agent
{
    public float eatTimer = 3.0f;
    public float grazingTimer = 5.0f;
    private Wander ankyWander;
    private Flee ankyFlee;
    private FieldOfView ankyView;
    private Transform enemyTarget;
    private Face ankyFace;
    private FaceEnemy ankyFaceEnemy;
    private Pursue ankyPursue;
    public Seek ankySeek;
    private Agent ankyAgent;
    private float enemyDistance;
    public float thirst;
    public float health;
    public float energy;
    private List<Transform> raptorLocations = new List<Transform>();
    private List<Transform> ankyLocations = new List<Transform>();
    private Transform closestRaptor;
    private Transform closestAnky;
    private float raptorDistance = 9999999999;
    private float ankyDistance = 9999999999;
    private float fleeDistance;
    private float herdDistance;
    private bool alive = true;
    public GameObject waterSource;
    

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

    public ankyState currentState;
    public ankyState previousState;
    public Animator anim;

    // Use this for initialization
    protected override void Start()
    {
        waterSource = GameObject.Find("Daylight Water");
        thirst = 100;
        health = 100;
        energy = 100;
        currentState = ankyState.IDLE;
        ankyWander = GetComponent<Wander>();
        ankyFlee = GetComponent<Flee>();
        ankyView = GetComponent<FieldOfView>();
        ankyFace = GetComponent<Face>();
        ankyFaceEnemy = GetComponent<FaceEnemy>();
        ankyPursue = GetComponent<Pursue>();
        ankySeek = GetComponent<Seek>();
        ankyAgent = GetComponent<Agent>();
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
        if (alive)
        {
            CheckSurroundings();
            UpdateStats();

            switch (currentState)
            {
                case ankyState.EATING:
                    Eating();
                    SetAnim();
                    break;
                case ankyState.DRINKING:
                    Drinking();
                    SetAnim();
                    break;
                case ankyState.ALERTED:
                    Alerted();
                    SetAnim();
                    break;
                case ankyState.GRAZING:
                    Grazing();
                    SetAnim();
                    break;
                case ankyState.ATTACKING:
                    SetAnim();
                    break;
                case ankyState.FLEEING:
                    Fleeing();
                    SetAnim();
                    break;
                case ankyState.DEAD:
                    Dead();
                    SetAnim();
                    break;
            }
        }

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    void SetState(ankyState newState)
    {
        if (currentState != newState)
        {
            previousState = currentState;
            currentState = newState;
        }
    }

    void SetAnim()
    {
        switch (previousState)
        {
            case ankyState.IDLE:
                anim.SetBool("isIdle", false);
                break;
            case ankyState.EATING:
                anim.SetBool("isEating", false);
                break;
            case ankyState.DRINKING:
                ankySeek.enabled = false;
                anim.SetBool("isDrinking", false);
                break;
            case ankyState.ALERTED:
                ankyFaceEnemy.enabled = false;
                anim.SetBool("isAlerted", false);
                break;
            case ankyState.GRAZING:
                ankyWander.enabled = false;
                ankySeek.enabled = false;
                ankyFace.enabled = false;
                anim.SetBool("isGrazing", false);
                break;
            case ankyState.ATTACKING:
                anim.SetBool("isAttacking", false);
                break;
            case ankyState.FLEEING:
                ankyFlee.enabled = false;
                anim.SetBool("isFleeing", false);
                break;
            case ankyState.DEAD:
                anim.SetBool("isDead", false);
                break;
        }

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
    }

    void Grazing()
    {
        if (thirst < 50)
        {
            SetState(ankyState.DRINKING);
        }
        else
        {
            if (herdDistance > 30)
            {
                ankyWander.enabled = false;
                FindHerd();
            }
            else
            {
                ankySeek.enabled = false;
                grazingTimer -= Time.deltaTime;
                ankyWander.enabled = true;

                if (grazingTimer <= 0)
                {
                    grazingTimer = 5.0f;
                    SetState(ankyState.EATING);
                }
            }
        }
    }

    void Eating()
    {
        eatTimer -= Time.deltaTime;
        energy += Time.deltaTime * 3;
        ankyWander.enabled = false;

        if (eatTimer <= 0)
        {
            eatTimer = 3.0f;
            SetState(ankyState.GRAZING); 
        }

        if (energy > 100)
            energy = 100;
    }

    void CheckSurroundings()
    {
        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            if (ankyView.visibleTargets[i].transform.name == "Rapty")
                raptorLocations.Add(ankyView.visibleTargets[i]);

            if (ankyView.visibleTargets[i].transform.name == "Anky")
                ankyLocations.Add(ankyView.visibleTargets[i]);
        }

        for (int i = 0; i < raptorLocations.Count; i++)
        {
            if (Vector3.Distance(raptorLocations[i].position, transform.position) < raptorDistance)
            {
                raptorDistance = Vector3.Distance(raptorLocations[i].position, transform.position);
                closestRaptor = raptorLocations[i];
                fleeDistance = raptorDistance;
            }
        }

        for (int i = 0; i < ankyLocations.Count; i++)
        {
            if (Vector3.Distance(ankyLocations[i].position, transform.position) < ankyDistance)
            {
                ankyDistance = Vector3.Distance(ankyLocations[i].position, transform.position);
                closestAnky = ankyLocations[i];
                herdDistance = ankyDistance;
            }
        }

        raptorLocations.Clear();
        ankyLocations.Clear();
        ReactToSurroundings();
    }

    void ReactToSurroundings()
    {
        if (raptorDistance <= 80 && raptorDistance > 40 && currentState != ankyState.FLEEING)
        {
            SetState(ankyState.ALERTED);
        }
        else if (raptorDistance <= 40 && raptorDistance > 10)
        {
            SetState(ankyState.FLEEING);
        }
        else if (raptorDistance <= 10)
        {
            SetState(ankyState.ATTACKING);
        }
        else
        {
            if (currentState != ankyState.EATING || currentState != ankyState.DRINKING)
                SetState(ankyState.GRAZING);
        }
        Debug.Log(raptorDistance);
        raptorDistance = 99999;
        ankyDistance = 99999;
    }

    void FindHerd()
    {
        ankySeek.target = closestAnky.gameObject;

        if (!ankySeek.enabled)
            ankySeek.enabled = true;
    }

    void Alerted()
    {     
        FaceEnemy.enemyTarget = closestRaptor;
        if (!ankyFaceEnemy.enabled)
            ankyFaceEnemy.enabled = true;
    }

    void Fleeing()
    {
        ankyAgent.maxSpeed = 3;
        energy -= Time.deltaTime * 2;
        Flee.enemyTarget = closestRaptor;
        if (!ankyFlee.enabled)
            ankyFlee.enabled = true;

        if (fleeDistance > 100)
            SetState(ankyState.GRAZING);
    }

    void Drinking()
    {
        ankySeek.target = waterSource;
        if (!ankySeek.enabled)
            ankySeek.enabled = true;

        if (transform.position.y < 36)
        {
            ankySeek.enabled = false;
            thirst += Time.deltaTime * 6;
        }

        if (thirst >= 100)
        {
            thirst = 100;
            SetState(ankyState.GRAZING);
        }
    }

    void UpdateStats()
    {
        thirst -= Time.deltaTime;
        energy -= Time.deltaTime / 2;

        if (thirst <= 0)
        {
            thirst = 0;
            health -= Time.deltaTime;
        }

        if (energy <= 0)
        {
            energy = 0;
            health -= Time.deltaTime;
        }

        if (health <= 0)
        {
            SetState(ankyState.DEAD);
        }
    }

    void Dead()
    {
        alive = false;
        DestroyObject(this.gameObject);
    }
}
