using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DinoStateMachine;


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
    /// <summary>
    /// /////////////////////////////////////////////Sam Maidment
    /// </summary>


    public bool switchState = false;
    public double gameTimer;
    public double seconds = 0;
    public double Health_Anky = 100;
    public double Anky_Water = 100;
    public double Anky_Food_Level = 100; // Declared Anky Food Level and set the value to 100. 
    public FieldOfView FieldOV;
    public GameObject Water;
    public GameObject Food;
    public GameObject rapty = null;
    
    public Seek SeekScript;
    public Wander WanderScript;
    public Flee fleeScript;
    public List<Transform> RaptorsInView = new List<Transform>();
    public Transform AnkyPos;

    public Anky_Behaviour<MyAnky> stateMachine { get; set; }
    public Animator anim;

    // Use this for initialization

    void Awake()
    {

        fleeScript = GetComponent<Flee>();
        WanderScript = GetComponent<Wander>();
        SeekScript = GetComponent<Seek>();
        FieldOV = GetComponent<FieldOfView>();
    }

    public void FieldOFView()
    {

        RaptorsInView = new List<Transform>();
        Vector3 VisDistance = new Vector3(1000, 1000, 1000);
        int index = 0;
      //  GameObject Dino = null;

        foreach (Transform i in FieldOV.visibleTargets)
        {


            if (i.tag == "Rapty" && !RaptorsInView.Contains(i))
            {

            RaptorsInView.Add(i);      

            }


        }

        RaptorsInView.Clear();

        foreach (Transform i in FieldOV.stereoVisibleTargets)
        {


            if (i.tag == "Rapty" && !RaptorsInView.Contains(i))
            {


                RaptorsInView.Add(i);
            }

        }
        rapty = null;

        for (int i = 0; i < RaptorsInView.Count; i++) {

            Distance = (AnkyPos.position - RaptorsInView[i].position);

            if (Distance.magnitude < VisDistance.magnitude) {


                VisDistance = Distance;
                index = i;

                rapty = RaptorsInView[index].gameObject;



            }



        }

        
    }
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
        stateMachine = new Anky_Behaviour<MyAnky>(this);
        stateMachine.ChangeDinoState(GrazingState.Instance);
        //anim = GetComponent<>();
        gameTimer = Time.time;

        SeekScript.target = Water;
     //   fleeScript.target = 

        

         

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
        // if (Anky_Water > 0)


        // Anky_Water = Anky_Water - 1;

        /*  if (Anky_Water == 100)
          {
              Anky_Water -= (Time.deltaTime * 0.3) * 1;


          }*/

        // for each time a second passes lower health water food levels by one.




        /*  if (Anky_Water == 100)
       {
           Anky_Water -= (Time.deltaTime * 0.3) * 1;


       }
       */

        if (Anky_Water >= 0)
        {

            Anky_Water -= (Time.deltaTime * 0.3) * 1;

        }

        if (Anky_Food_Level >= 0)
        {

            Anky_Food_Level -= (Time.deltaTime * 0.3) * 1;

        }



        if (Anky_Water <= 0 || Anky_Food_Level <= 0) {

            Health_Anky -= (Time.deltaTime*0.3)*1;


        }


     FieldOFView();


        stateMachine.Update();
        base.Update();
    }
    //Debug.Log("");

    public Vector3 Distance = new Vector3();
    protected override void LateUpdate()
    {

        base.LateUpdate();
    }


}


