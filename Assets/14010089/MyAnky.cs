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

    public GameObject WaterSource;

    public double AnkyWaterLevel = 100;
    public double AnkyFoodLevel = 100;
    private const float WaterCoef = 0.6f;
    private const float FoodCoef = 0.2f;
    private const float RecoverWaterCoefMultiplier = 10f;
    private const float RecoverFoodCoefMultiplier = 5f;

    public Animator anim;
    public FieldOfView fov;

    public List<Transform> FovRaptorList = new List<Transform>();
    public List<Transform> FovAnkyList = new List<Transform>();

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
                FovAnkyList.Add(i);
            }

            if (i.gameObject.tag == "Rapty")
            {
                FovRaptorList.Add(i);
            }
            
        }
        #endregion

        #region FieldofView_StereoVisibleTargets
        foreach (Transform i in fov.stereoVisibleTargets)
        {
            /*
            if (i.gameObject.tag == "Anky")
            {
                FovAnkyList.Add(i);
            }

            if (i.gameObject.tag == "Rapty")
            {
                FovRaptorList.Add(i);
            }
            */
        }

        Debug.Log("Number of Raptors insight: " + FovRaptorList.Count);
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

            if (FovRaptorList.Count > 0)
            {
                Wander.enabled = false;
                anim.SetBool("isGrazing", false);
                anim.SetBool("isAlerted", true);

            }

            if (AnkyWaterLevel <= 30)
            {
                Wander.enabled = false;
                anim.SetBool("isGrazing", false);
                anim.SetBool("isDrinking", true);
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

                    if (FovRaptorList.Count <= 0)
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
            Flee.enabled = true;
            Debug.Log("Anky is Alerted");

            if (FovRaptorList.Count <= 0)
            {
                Flee.enabled = false;
                anim.SetBool("isAlerted", false);
                anim.SetBool("isGrazing", true);
            }

        }
        #endregion




        AnkyFoodLevel -= FoodCoef * Time.deltaTime;
        AnkyWaterLevel -= WaterCoef * Time.deltaTime;

        FovRaptorList.Clear();
        base.Update();
    }

    #region Old
    /*
protected override void Update()
{

    #region FieldofView_VisibleTargets

    foreach (Transform i in fov.visibleTargets)
    {
        if (i.gameObject.tag == "Anky")
        {
            FovAnkyList.Add(i);
        }

        if (i.gameObject.tag == "Rapty")
        {
            FovRaptorList.Add(i);
        }

    }
    #endregion

    #region FieldofView_StereoVisibleTargets
    foreach (Transform i in fov.stereoVisibleTargets)
    {
        if (i.gameObject.tag == "Anky")
        {
            FovAnkyList.Add(i);
        }

        if (i.gameObject.tag == "Rapty")
        {
            FovRaptorList.Add(i);
        }
    }

    Debug.Log("Number of Raptors insight: " + FovRaptorList.Count);
    #endregion

    AnkyFoodLevel -= FoodCoef * Time.deltaTime;
    AnkyWaterLevel -= WaterCoef * Time.deltaTime;

    #region States

    #region IDLE
    #region IDLEtoGRAZING
    // Idle - should only be used at startup
    if (anim.GetBool("isIdle"))
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isGrazing", true);
    }
    #endregion
    #endregion

    #region GRAZING
    if (anim.GetBool("isGrazing"))
    {
        Wander.enabled = true;

        #region GRAZINGtoALERTED
        if (FovRaptorList.Count > 0)
        {
            Wander.enabled = false;
            anim.SetBool("isGrazing", false);
            anim.SetBool("isAlerted", true);

        }
        #endregion

        #region GRAZINGtoDRINKING
        if (AnkyWaterLevel <= 90)
        {
            Wander.enabled = false;
            anim.SetBool("isGrazing", false);
            anim.SetBool("isDrinking", true);

        }
        #endregion
    }

    #endregion


    #region DRINKING

    if (anim.GetBool("isDrinking"))
    {
        Debug.Log("Anky is Going to seek water");
        Seek.enabled = true;

        #region Seek&Drink
        if (transform.position.y <= 36)
        {
            Seek.enabled = false;
            AnkyWaterLevel += RecoverWaterCoefMultiplier * Time.deltaTime;
            Debug.Log("Anky is drinking");
        }
        #endregion

        if (AnkyWaterLevel >= 100)
        {
            #region DRINKINGtoGRAZING
            if (FovRaptorList.Count <= 0)
            {
                Debug.Log("Anky not thirsty, reverting to grazing");
                anim.SetBool("isDrinking", false);
                anim.SetBool("isGrazing", true);
            }
            #endregion
            else
            #region DRINKINGtoALERTED
            {
                Debug.Log("Anky not thirsty, reverting to Alerted");
                anim.SetBool("isDrinking", false);
                anim.SetBool("isAlerted", true);
            }
            #endregion
        }
    }

    #endregion

    #region ALERTED
    // Alerted - up to the student what you do here

    if (anim.GetBool("isAlerted"))
    {
        Flee.enabled = true;
        Debug.Log("Anky is Alerted");
    }
    else
    {
        anim.SetBool("isAlerted", false);
        anim.SetBool("isDrinking", true);
    }


    #endregion

    #region EATING
    // Eating - requires a box collision with a dead dino
    #endregion

    #region HUNTING
    // Hunting - up to the student what you do here
    #endregion

    #region FLEEING
    // Fleeing - up to the student what you do here
    #endregion

    #region DEAD
    // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
    #endregion

    #endregion

    FovRaptorList.Clear();
    base.Update();
}
*/

    #endregion

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
