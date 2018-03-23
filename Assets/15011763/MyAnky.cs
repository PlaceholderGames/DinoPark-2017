using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using myStateMachine;

public class MyAnky : Agent
{


    // Reference to the State Machine
    public StateMachine<MyAnky> myStateMachine { get; set; } 

    // Vision Lists
    public List<Transform> enemyList;
    public List<Transform> teamList;

    // References to Anky Components
    public MyAnky myAnky;
    public Agent myAnkyAgent;
    public Flee myAnkyFlee;
    public Wander myAnkyWander;
    public ReverseFlee myAnkyHerd;
    public AStarSearch myAnkyAStar;
    public ASAgentInstance myAnkyAStarInstance;
    public ASPathFollower myAnkyPathFollower;
    public FieldOfView myView;
    public Animator myAnim;

    // Water / Food Object Lists
    public List<Transform> thirstyList;
    public List<Transform> hungryList;

    // Reference to enemyDino Dino and Distance
    public GameObject enemyDino = null;
    public float enemyDistance;

    // Anky Variables
    public float hungryDino = 100;
    public float healthyDino = 100;
    public float thirstyDino = 100;

    // Reference to Team Dino and 
    public GameObject teamDino = null;
    public float teamDistance;

    // Reference to this dino's transform
    public Transform myTransform; 
    
    // Use this for initialization
    protected override void Start()
    {
        // Initalise State Machine
        myStateMachine = new StateMachine<MyAnky>(this);

        // Initalise myAnky's Components
        myAnky = GetComponent<MyAnky>();
        myAnkyAgent = GetComponent<Agent>();
        myAnkyFlee = GetComponent<Flee>();
        myAnkyWander = GetComponent<Wander>();
        myAnkyHerd = GetComponent<ReverseFlee>();
        myAnkyAStar = GetComponent<AStarSearch>();
        myAnkyAStarInstance = GetComponent<ASAgentInstance>();
        myAnkyPathFollower = GetComponent<ASPathFollower>();
        myAnim = GetComponent<Animator>();
        myView = GetComponent<FieldOfView>();

        // Initalise my transform
        myTransform = transform;

        // Initalise the enemy and team lists
        enemyList = new List<Transform>();
        teamList = new List<Transform>();

        // Call the handle variables function
        // This is used to handle the anky's hungryDino, thirstyDino and hunger
        HandleVariables();

        // Change to Idle State
        myStateMachine.ChangeState(Idle.Instance);
        
        base.Start();
    }

    // Decrease Hunger
    IEnumerator decreaseHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            if (hungryDino > 0)
                hungryDino = hungryDino - 0.1f;
            else
            {
                break;
            }
            
        }
    }
    // Decrease Thirst
    IEnumerator decreaseThirst()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            if (thirstyDino > 0)
                thirstyDino = thirstyDino - 0.1f;
        }
    }
    // Decrease Health
    IEnumerator decreaseHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (healthyDino > 0)
                if (thirstyDino == 0 || hungryDino == 0)
                    healthyDino = healthyDino - 2.0f;
        }
    }
    // Increase Health
    IEnumerator increaseHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            if (healthyDino != 0 && healthyDino < 100)
                healthyDino = healthyDino + 1.0f;
        }
    }
    // Update Function
    protected override void Update()
    {
        // Scan field of view
        scanForTeam();
        scanForEnemy();
        
        // Call the Current State's Update
        myStateMachine.Update();

        base.Update();
    }
    public void scanForTeam()
    {
        // Searches for the team member that is the furthest away
        // Reinitalise the teamList
        teamList = new List<Transform>();

        // aDistance is used to compare distances
        // aIndex is used to hold the index of the list that is returned
        float aDistance = 1;
        int aIndex = 0;
        GameObject teamMember = null;

        // Loop through the visible targets list and return visable Ankys
        for (int i = 0; i < myView.visibleTargets.Count; i++)
        {
            if (myView.visibleTargets[i].gameObject.tag == "Anky")
            {
                teamList.Add(myView.visibleTargets[i]);
            }
        }
        // Loop through list of all found dinos and compare the distances

            for (int i = 0; i < teamList.Count; i++)
            {
                teamDistance = Vector3.Distance(myTransform.position, teamList[i].position);

                if (teamDistance > aDistance)
                {
                    aDistance = teamDistance;
                    aIndex = i;
                    teamMember = teamList[aIndex].gameObject;
                }
            }


        if (teamMember != null)
        {
            teamDino = teamMember;
            teamDistance = aDistance;
        }
        else
        {
            teamDino = null;
        }
    }
    public void scanForEnemy()
    {
        // Searches for the enemy that is the closest
        // Reinitalise the teamList
        enemyList = new List<Transform>();
        // rDistance is used to compare distances
        // rIndex is used to hold the index of the list that is returned
        float rDistance = 10000;
        int rIndex = 0;
        // Closest enemy
        GameObject enemyMember = null;
        // Loop through the visible targets list and return visable Raptys
        for (int i = 0; i < myView.visibleTargets.Count; i++)
        {
            if (myView.visibleTargets[i].gameObject.tag == "Rapty")
            {
                enemyList.Add(myView.visibleTargets[i]);
            }
        }
        // Loop through the stereo visible targets list and return visable Raptys
        for (int i = 0; i < myView.stereoVisibleTargets.Count; i++)
        {
            if (myView.stereoVisibleTargets[i].gameObject.tag == "Rapty")
            {
                enemyList.Add(myView.stereoVisibleTargets[i]);
            }
        }
        // Loop through list of all found dinos and compare the distances
        if (enemyList.Count > 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyDistance = Vector3.Distance(myTransform.position, enemyList[i].position);

                if (enemyDistance < rDistance)
                {
                    rDistance = enemyDistance;
                    rIndex = i;
                    enemyMember = enemyList[rIndex].gameObject;
                }
            }

            enemyDino = enemyMember;
            enemyDistance = rDistance;
        }
        else
        {
            enemyDino = null;
            enemyDistance = -1;
        }

    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    // Handle the changing variables
    public void HandleVariables()
    {
        if (hungryDino > 0)
        {
            StartCoroutine(decreaseHunger());
        }

        if (healthyDino > 0 && healthyDino <= 100)
        {
            StartCoroutine(increaseHealth());
        }

        if (thirstyDino > 0)
        {
            StartCoroutine(decreaseThirst());
        }

        if (healthyDino > 0)
        {
                StartCoroutine(decreaseHealth());        
        }

    }
}
