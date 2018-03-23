using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAnky : Agent
{
    public float health = 100;// to say wether anky is alive or not 
    public float thirst = 40;// to track when a anky needs a drink if 0 he dies slowly
    private float hunger = 100;// to track hunger when 0 dies slowly
    private List<Transform> hunters = new List<Transform>();
    private bool can_see_hunter = false;  //  to tell us if a hunter is found
    private bool can_see_friend = false; // tells us if there is at least one friendly dino
    private bool only_need_hunter = false;//if we only need to find one hunter then this skips the rest of a foreach loop when on is found 
    private bool only_need_friend = false;// same as  above but only for friends.
    private float speed = 10.0f;
    private float walking_time = 5.0f;
    Vector3 waterPos = new Vector3(0,32,0) ;
    Vector3  ankyY = new Vector3(0,0,0);
    public enum ankyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on y value of the object to denote grass level
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        GRAZING,    // Moving with the intent to find food (will happen after a random period)
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        WANDERING, // when the anky is just wandering around
        DEAD // only set when dino is dead
    };


    public Animator anim;
    public ankyState currentState;
    private FieldOfView sight;
    private Wander wandering;
    private Flee fleeing;
    private Attack attacking;
    private AStarSearch AStar;
    private Transform target;
    private Face facetime;
    private Seek seekout;

    // Use this for initialization
    protected override void Start()
    {


        anim = GetComponent<Animator>();//load in animator 
        sight = GetComponent<FieldOfView>();//load in field of view 
        fleeing = GetComponent<Flee>();// load in flee action
        wandering = GetComponent<Wander>(); // load in wander action
        AStar = GetComponent<AStarSearch>(); // loads in a star search
        facetime = GetComponent<Face>(); //loads in face script
        seekout = GetComponent<Seek>(); //  loads in seek
       

        //Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        //This with GetBool and GetFloat allows 
        //you to see how to change the flag parameters in the animation controller
        AStar.enabled = false;
        fleeing.enabled = false;
        wandering.enabled = false;
        seekout.enabled = false;

        currentState = ankyState.IDLE;// anky will always start the game idle;

        base.Start();

    }

    protected override void Update()
    {
        if (health <= 0)
            currentState = ankyState.DEAD;
        if (thirst > 0 && currentState != ankyState.DRINKING)
        {//makes him thristy every update
            thirst -= Time.deltaTime;
           // Debug.Log("thirst at: " + thirst);
        }
        //makes him more hungry each update
            if (hunger > 0)
            {
                hunger -= Time.deltaTime;
            }
        //will start to hurt him if too thirsty or hungry
            if (thirst == 0)
            {
                health -= Time.deltaTime;
            }

        lookAround();//looking for raptors

        //IDLE STATE
        if (currentState == ankyState.IDLE)
        {
            if (!can_see_hunter) { currentState = ankyState.WANDERING; Debug.Log("going into " + currentState); }
            else
            {
                Debug.Log("going into " + currentState);
                currentState = ankyState.ALERTED;
            }

        }
        //WANDERING STATE
        else if (currentState == ankyState.WANDERING)
        {
            if (this.transform.position.y < 50)
            {
                
            }
            if (!wandering.enabled)
            {
                wandering.enabled = true;
                fleeing.enabled = false;
            }
            if (can_see_hunter) { currentState = ankyState.ALERTED; Debug.Log("going into " + currentState); }
            else if (thirst < 30)
            {
                currentState = ankyState.DRINKING;
                Debug.Log("going into " + currentState);
            }
        }

            //ALERTED STATE
        else if (currentState == ankyState.ALERTED)
        {
           
            if (!can_see_hunter)
            {
                Debug.Log("going into " + currentState);
                currentState = ankyState.WANDERING; 
            }
            else
            {
                find_nearest();
                if (flee_or_not())
                {
                    Debug.Log("going into " + currentState);
                    currentState = ankyState.FLEEING;
                }
                else if (!wandering.enabled)
                {
                    wandering.enabled = true; fleeing.enabled = false;
                }
            }
        }
        //FLEEING STATE     
        else if (currentState == ankyState.FLEEING)
        {
            if (!fleeing.enabled)//turn on the run away 
            {
                fleeing.enabled = true;
                wandering.enabled = false;
            };

            if (!flee_or_not())
            {
                if (!can_see_hunter)
                {
                    currentState = ankyState.WANDERING;
                    Debug.Log("going into " + currentState);
                }
                else
                {
                    currentState = ankyState.ALERTED;
                    Debug.Log("going into " + currentState);
                }
            }
        }
        //drinking state
        else if (currentState == ankyState.DRINKING)
        {
            wandering.enabled = false;
            AStar.enabled = true;
            if (thirst >= 100)//if we have a full thirst then we stop drinking and wander
            {
                currentState = ankyState.WANDERING;
                Debug.Log("going into " + currentState);
               // seekout.enabled = false;
               AStar.enabled = false;
            }
             else if (can_see_hunter)//if we can see a hunter we stop drinking and go into alert 
            {
                currentState = ankyState.ALERTED;
                Debug.Log("going into " + currentState);
                //seekout.enabled = false;
                AStar.enabled = false;
            }
            if (AStar.path.nodes.Count <= 0)
            {
                AStar.enabled = false;
                if (this.transform.position.y <= 35)// we knows he is at water and so he drinks 
                {
                    Debug.Log("im drinking");
                    seekout.enabled = false;
                    thirst += Time.deltaTime;

                }
                else if (this.transform.position.y > 35 && thirst < 30)//he is using a* to find the water
                {
                    seekout.enabled = true;
                    Debug.Log("looking for the water");
                }
            }
            else
            {
                AStar.enabled = true;
            }
         
        }
        //dead state
        else if (currentState == ankyState.DEAD)
        {
            Debug.Log("anky is  dead");
        }

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    /// <summary>
    /// looks for raptors and adds them into lists
    /// </summary>
    void lookAround()
    {
       // Debug.Log("I am looking around");
        can_see_hunter = false;
        can_see_friend = false;


        hunters.Clear();
        foreach (Transform i in sight.visibleTargets)//go though all dinos in sight 
        {
            //Transform target = sight.visibleTargets[i];
            if (i.tag == "Rapty" && !hunters.Contains(i))
            {

                can_see_hunter = true;
                hunters.Add(i);
            }
        }
        foreach (Transform i in sight.stereoVisibleTargets)//go though all dinos in sight 
        {
            if (i.tag == "Rapty" && !hunters.Contains(i))
            {

                can_see_hunter = true;
                hunters.Add(i);
            }
        }
        //Debug.Log(hunters.Count);// print out the number of visible raptors to the console
    }


    /// <summary>
    /// finds the nearst of a group and returns it 
    /// </summary>
    void find_nearest()
    {
        target = null;
        if (hunters.Count < 1)
        {
        target = null;
            Debug.Log("hunters is empty  size of hunters is:" + hunters.Count);
        }
        else    
        {
        Transform nearestHunter = hunters[0];
        float nearestDistance = Vector3.Distance(hunters[0].transform.position, this.transform.position);
            for(int i = 1; i < hunters.Count; i++)
            {
                if (nearestDistance > Vector3.Distance(hunters[i].transform.position, this.transform.position))
                {
                    nearestDistance = Vector3.Distance(hunters[i].transform.position, this.transform.position);
                    nearestHunter = hunters[i];
                }
            }
            target = nearestHunter;
        }
        fleeing.target = target.gameObject;
    }
            
    /// <summary>
    /// checks how close a hunter is and if to run or not 
    /// </summary>
    /// <returns></returns>
    bool flee_or_not()
    {
        if ((Vector3.Distance(target.transform.position,this.transform.position)) <= 30)
        {
            return true;
        }
        else
            return false;
    }

    void aStar_to_land()
    {
       // AStar.target = 
    }


}