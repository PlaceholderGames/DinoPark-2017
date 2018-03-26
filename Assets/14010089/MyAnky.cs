using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAnky : Agent
{

    #region Enums
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
    #endregion

    #region PublicVariables

    public IdlePatrol Patrol;
    public Wander Wander;
    public Flee Flee;
    public FaceAway FaceAway;
    public Face Face;
    public Seek Seek;
    public health health;

    public GameObject WaterSource;
    public static GameObject[] waypoints;
    public GameObject[] myStool;

    public Transform stoolPos;

    public double AnkyWaterLevel = 100;
    public double AnkyFoodLevel = 100;
    private const float WaterCoef = 0.6f;
    private const float FoodCoef = 0.2f;
    private const float RecoverWaterCoefMultiplier = 10f;
    private const float RecoverFoodCoefMultiplier = 5f;
    public int currentWP = 0;


    public Animator anim;
    public FieldOfView fov;

   // public List<Transform> FovRaptorList = new List<Transform>();
   // public List<Transform> FovAnkyList = new List<Transform>();


    public List<Transform> FovMonoRaptorList = new List<Transform>();
    public List<Transform> FovMonoAnkyList = new List<Transform>();

    public List<Transform> FovStereoRaptorList = new List<Transform>();
    public List<Transform> FovStereoAnkyList = new List<Transform>();



    #endregion

    // Use this for initialization
    protected override void Start()
    {
        #region Components
        Patrol = GetComponent<IdlePatrol>();
        Wander = GetComponent<Wander>();
        Flee = GetComponent<Flee>();
        FaceAway = GetComponent<FaceAway>();
        Face = GetComponent<Face>();
        Seek = GetComponent<Seek>();
        anim = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();

        health = GetComponent<health>();


        waypoints = GameObject.FindGameObjectsWithTag("waypoints");
        //waypoints = GameObject.FindGameObjectsWithTag("waypoint1");

        #endregion

        #region AnimationStateBooleans
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
        #endregion

        base.Start();

    }



    protected override void Update()
    {

        #region FieldofView_VisibleTargets

        foreach (Transform i in fov.visibleTargets)
        {
            
            if (i.gameObject.tag == "Anky")
            {
                FovMonoAnkyList.Add(i);
            }

            if (i.gameObject.tag == "Rapty")
            {
                FovMonoRaptorList.Add(i);
            }
            
        }
        #endregion

        #region FieldofView_StereoVisibleTargets
        foreach (Transform i in fov.stereoVisibleTargets)
        {
            
            if (i.gameObject.tag == "Anky")
            {
                FovStereoAnkyList.Add(i);
            }

            if (i.gameObject.tag == "Rapty")
            {
                FovStereoRaptorList.Add(i);
            }
            
        }

        Debug.Log("Number of Raptors in Mono vision of anky: " + FovMonoRaptorList.Count);
        Debug.Log("Number of Raptors insight in stereo vision of anky: " + FovStereoRaptorList.Count);
        #endregion

        #region IDLE
        if (anim.GetBool("isIdle"))
        {
                anim.SetBool("isIdle", false);
                anim.SetBool("isGrazing", true);
        }
        #endregion

        #region Grazing
        if (anim.GetBool("isGrazing"))
        {
            Wander.enabled = true;
            Debug.Log("Anky is Grazing");

            if (FovMonoRaptorList.Count > 0)
            {
                Wander.enabled = false;
                anim.SetBool("isGrazing", false);
                anim.SetBool("isAlerted", true);
            }

            if (FovStereoRaptorList.Count > 0)
            {
                Wander.enabled = false;
                anim.SetBool("isGrazing", false);
                anim.SetBool("isFleeing", true);
            }

            if (AnkyWaterLevel <= 30)
            {
                Wander.enabled = false;
                anim.SetBool("isGrazing", false);
                anim.SetBool("isDrinking", true);
            }

            if (AnkyFoodLevel <= 40)
            {
                Wander.enabled = false;
                anim.SetBool("isGrazing", false);
                anim.SetBool("isEating", true);
            }

        }
        #endregion

        #region Drinking
        if (anim.GetBool("isDrinking"))
        {
            Seek.enabled = true;
            Debug.Log("Anky is Going to seek water");

            if (transform.position.y <= 36)
            {
                Seek.enabled = false;
                Debug.Log("Anky is drinking");
                AnkyWaterLevel += RecoverWaterCoefMultiplier * Time.deltaTime;

                if (AnkyWaterLevel >= 100)
                {
                    Debug.Log("Anky no longer thirsty");

                    if (FovMonoRaptorList.Count <= 0)
                    {
                        anim.SetBool("isDrinking", false);
                        anim.SetBool("isGrazing", true);
                    }
                    else
                    {
                        anim.SetBool("isDrinking", false);
                        anim.SetBool("isAlerted", true);
                    }
                }
            }
        }
        #endregion

        #region Alerted
        if (anim.GetBool("isAlerted"))
        {
            Debug.Log("Anky is Alerted");
            Wander.enabled = true;
            Wander.rate = 100;

            if ((FovMonoRaptorList.Count <= 0) && (FovStereoRaptorList.Count <=0))
            {
                anim.SetBool("isAlerted", false);
                anim.SetBool("isGrazing", true);
            }

            if (FovStereoRaptorList.Count > 0)
            {
                anim.SetBool("isAlerted", false);
                anim.SetBool("isFleeing", true);
            }

        }
        #endregion

        #region Eating
        if (anim.GetBool("isEating"))
        {
            Debug.Log("Anky is Going to seek food");

            transform.position = Vector3.MoveTowards(transform.position, 
                                                        waypoints[currentWP].transform.position, 
                                                            2 * Time.deltaTime);

            if (Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < 1.0f)
            {
                Destroy(waypoints[currentWP]);
                Debug.Log(waypoints[currentWP]);
                currentWP++;
                AnkyFoodLevel += +40;
            }

            if (AnkyFoodLevel >= 100)
            {
                AnkyFoodLevel = 100;
                Debug.Log("Anky no longer hungry");

                if (FovMonoRaptorList.Count <= 0)
                {
                    anim.SetBool("isEating", false);
                    anim.SetBool("isGrazing", true);
                }
                else
                {
                    anim.SetBool("isEating", false);
                    anim.SetBool("isAlerted", true);
                }
            }
        }
        #endregion

        #region FLEEING
        // Fleeing - up to the student what you do here
        if (anim.GetBool("isFleeing"))
        {
            Flee.enabled = true;
            Debug.Log("Anky is Fleeing");

            if (FovStereoRaptorList.Count <= 0)
            {
                Flee.enabled = false;
                Debug.Log("Anky lost sight of Rapty");
                anim.SetBool("isFleeing", false);
                anim.SetBool("isAlerted", true);
            }
        }
        #endregion

        #region HUNTING
        // Hunting - up to the student what you do here
        #endregion

        #region DEAD
        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
        #endregion

        AnkyFoodLevel -= FoodCoef * Time.deltaTime;
        AnkyWaterLevel -= WaterCoef * Time.deltaTime;

        FovMonoRaptorList.Clear();
        FovMonoAnkyList.Clear();
        FovStereoRaptorList.Clear();
        FovStereoAnkyList.Clear();
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
