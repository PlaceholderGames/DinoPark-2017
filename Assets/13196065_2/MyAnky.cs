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

    public float hunger = 100;
    private float GrazingTime = 5;
    private float eatingTime = 3;
    public float health = 8.0f;
    public float thirst = 100;
    private FieldOfView sight;
    public float RaptyDist;
    public float ankyDist;
    private bool isAlive = true;


    private Transform RaptyInSight;
    private Transform AnkyInSight;

    public GameObject water;
    public GameObject closestRapty;

    //  public GameObject closestRapty = new GameObject();

    public float raptyRange;

   // public string[] viewRapty = { "Rapty" };
   // public string[] viewAnky = { "Anky" };
    

    public List<Transform> viewRapty = new List<Transform>();
    public List<Transform> viewAnky = new List<Transform>();
    public float fleeDistance = 50;
    public float safeTime = 4;
    private float Run = 0;


    //public Transform closestHazard = null;



    private Wander ankyWander;
    private Flee ankyFlee;
    private Face ankyFace;
    public Seek ankySeek;


    // Use this for initialization
    protected override void Start()
    {
        ankyWander = GetComponent<Wander>();


        ankyFlee = GetComponent<Flee>();

        anim = GetComponent<Animator>();
        

        sight = GetComponent<FieldOfView>();

        ankyFace = GetComponent<Face>();

        ankySeek = GetComponent<Seek>();

        //RaptyRange = new List<Transform>();
        // AnkyRange = new List<Transform>();

        currentState = ankyState.GRAZING;

        //ankySeek.target = Water;

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
        if (isAlive)
        {

            if (thirst > 0)
            {
                thirst -= (Time.deltaTime * 2) * 1;
            }
            if (hunger > 0)
            {
                hunger -= (Time.deltaTime * 1) * 1;
            }


            if (thirst <= 0 && hunger <= 0)
            {
                health -= (Time.deltaTime * 1) * 1;
            }

            if (health <=0)
            {
                setState(ankyState.DEAD);

            }




            //foreach (Transform i in sight.visibleTargets)
            //{



            //    if (i.tag == "Rapty")
            //    {
            //        Debug.Log("Adding Raptor");
            //        viewRapty.Add(i);

            //    }
            //}


            //foreach (Transform u in sight.visibleTargets)
            //{
            //    if (u.tag == "Anky")
            //    {
            //        if (u.transform.position != transform.position)
            //        {
            // 

            Scan();

            if (currentState == ankyState.EATING)
            {
                Eating();
                setAnim();

            }
            // Drinking - requires y value to be below 32 (?)
            if (currentState == ankyState.DRINKING)
            {
                Drinking();
                setAnim();

            }
            // Alerted - up to the student what you do here
            if (currentState == ankyState.ALERTED)
            {

                AnkyAlert();
                setAnim();

            }
            // Hunting - up to the student what you do here
            if (currentState == ankyState.GRAZING)
            {

                Grazing();
                setAnim();

            }
            // Fleeing - up to the student what you do here
            if (currentState == ankyState.FLEEING)
            {
                // this.GetComponent<Flee>().target = this.gameObject;

                AnkyFlee();
                setAnim();
            }
            // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
            if (currentState == ankyState.DEAD)
            {
                Dead();
                setAnim();
            }




        }
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }


    void setState(ankyState newState)
    {
        if (currentState != newState)
        {
            previousState = currentState;
            currentState = newState;
        }

    }

    void setAnim()
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
                anim.SetBool("isAlerted", false);
                break;


            case ankyState.GRAZING:
                ankyWander.enabled = false;

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







    void Eating()
    {
        //hunger -= Time.deltaTime;
        ankyWander.enabled = false;

        //if (hunger <= 0)
        //{
        //    currentState = ankyState.GRAZING;
        //    hunger = 3.0f;
        //}

        hunger += (Time.deltaTime * 4) * 1;

        if (hunger >= 100)
        {
            setState(ankyState.GRAZING);

        }


    }

    void Drinking()
    {
        for (int i = 0; i < sight.visibleTargets.Count; i++)
        {
            // ankyWander.enabled = false;
            ankySeek.target = water;

            ankySeek.enabled = true;

            if (transform.position.y < 36)
            {
                ankySeek.enabled = false;
                thirst += (Time.deltaTime * 3) * 1;
            }

            if (thirst >= 100)
            {
                setState(ankyState.GRAZING);
                //currentState = ankyState.GRAZING;
            }




            if (sight.visibleTargets[i].tag == "Rapty")
            {
                RaptyInSight = sight.visibleTargets[i];
                RaptyDist = Vector3.Distance(RaptyInSight.position, this.transform.position);
            }

            if (sight.visibleTargets[i].transform.name == "Rapty" && RaptyDist <= 50)
            {
                RaptyInSight = sight.visibleTargets[i];
                setState(ankyState.ALERTED);
                //currentState = ankyState.ALERTED;

            }


        }

    }


    //void Eat()
    //{
    //    ankyWander.enabled = false;

    //    hunger += (Time.deltaTime * 2) * 1;

    //    if (hunger >= 100)
    //    {
    //        currentState = ankyState.GRAZING;
    //    }

    void Dead()
    {
        isAlive = false;
        DestroyObject(this.gameObject);
    }


    //}

    void Grazing()
    {
        for (int i = 0; i < sight.visibleTargets.Count; i++)
        {

            //if (thirst > 0)
            //{
            //    thirst -= (Time.deltaTime * 2) * 1;
            //}
            //if (hunger > 0)
            //{
            //    hunger -= (Time.deltaTime * 1) * 1;
            //}
            //  GrazingTime -= Time.deltaTime;
            ankyWander.enabled = true;

            if (thirst < 50)
            {
                setState(ankyState.DRINKING);
                //currentState = ankyState.DRINKING;


            }

            if (hunger < 30)
            {
                setState(ankyState.EATING);
            }


            Debug.Log("rapty in sight ");

            if (sight.visibleTargets[i].tag == "Rapty")
            {
                RaptyInSight = sight.visibleTargets[i];
                RaptyDist = Vector3.Distance(RaptyInSight.position, this.transform.position);
            }


            if (sight.visibleTargets[i].transform.name == "Rapty" && RaptyDist <= 50 )
            {
                RaptyInSight = sight.visibleTargets[i];
                setState(ankyState.ALERTED);
                //currentState = ankyState.ALERTED;

            }
        }


        foreach (Transform t in sight.visibleTargets)
        {
            float ankyDistance = Vector3.Distance(t.position,transform.position);


            if (ankyDistance > 50)
            {
                ankySeek.target = t.gameObject;
                ankySeek.enabled = true;
            }
            else if (ankyDistance < 20)
            {
                ankySeek.enabled = false;
                ankyWander.enabled = true;
            }

        }


    }

    //void Grazing()
    //{

    //    GrazingTime -= Time.deltaTime;
    //    ankyWander.enabled = true;

    //    if (GrazingTime <= 0)
    //    {
    //        currentState = ankyState.EATING;

    //        GrazingTime = 5.0f;
    //    }

    //}





    void Scan()
    {
        for (int i = 0; i < sight.visibleTargets.Count; i++)
        {

            Debug.Log("rapty in sight ");
            if (sight.visibleTargets[i].tag == "Rapty")
            {

                RaptyInSight = sight.visibleTargets[i];
                RaptyDist = Vector3.Distance(RaptyInSight.position, this.transform.position);
            }
        }

        for (int u = 0; u < sight.visibleTargets.Count; u++)
        {
            Debug.Log("anky in sight ");
            if (sight.visibleTargets[u].tag == "Anky")
            {

                AnkyInSight = sight.visibleTargets[u];
                ankyDist = Vector3.Distance(AnkyInSight.position, this.transform.position);
            }
        }

            //if (sight.visibleTargets[i].transform.name == "Rapty" && RaptyDist <= 150 && RaptyDist > 40)
            //{
            //    RaptyInSight = sight.visibleTargets[i];
            //    //setState(ankyState.ALERTED);
            //    currentState = ankyState.ALERTED;

            //}
            //else if (sight.visibleTargets[i].tag == "Rapty" && RaptyDist <= 40)
            //{
            //    RaptyInSight = sight.visibleTargets[i];
            //    // setState(ankyState.FLEEING);
            //    currentState = ankyState.FLEEING;
            //}
        
    }




    void AnkyAlert()
    {
        Debug.Log("ALERT");
        for (int i = 0; i < sight.visibleTargets.Count; i++)
        {
            
            Debug.Log("rapty in sight ");

            if (sight.visibleTargets[i].tag == "Rapty")
            {
                RaptyInSight = sight.visibleTargets[i];
                RaptyDist = Vector3.Distance(RaptyInSight.position, this.transform.position);
            }


                if (sight.visibleTargets[i].tag == "Rapty" && RaptyDist <= 50)
            {

                RaptyInSight = sight.visibleTargets[i];
                setState(ankyState.FLEEING);
                
              //  currentState = ankyState.FLEEING;

            }


            if (sight.visibleTargets[i].tag == "Rapty" && RaptyDist > 60)
            {

                RaptyInSight = sight.visibleTargets[i];
                setState(ankyState.GRAZING);

                //currentState = ankyState.GRAZING;

            }

        }
    }

    


    //void AnkyFlee()
    //{


    //    for (int i = 0; i < sight.visibleTargets.Count; i++)
    //    {
    //        if (sight.visibleTargets[i].tag == "Rapty")
    //        {
    //            Flee.RaptyInSight = RaptyInSight;


    //            ankyFlee.target = i.gameObject;
    //            if (!ankyFlee.enabled)
    //                ankyFlee.enabled = true;
    //            RaptyDist = Vector3.Distance(RaptyInSight.position, this.transform.position);
    //            if (RaptyDist > 50)
    //            {
    //                currentState = ankyState.ALERTED;

    //            }


    //        }


    //    }

    //}





    void AnkyFlee()
    {


        foreach (Transform i in sight.visibleTargets)
        {
            if (i.tag == "Rapty")
            {
                // Flee.RaptyInSight = RaptyInSight;


                ankyFlee.target = i.gameObject;
                // if (!ankyFlee.enabled)
                ankyFlee.enabled = true;
                RaptyDist = Vector3.Distance(RaptyInSight.position, this.transform.position);
                if (RaptyDist > 60)
                {
                    setState(ankyState.GRAZING);
                    //currentState = ankyState.GRAZING;

                }


            }


        }

    }


    //void AnkyFlee()
    //{


    //    foreach (Transform i in sight.stereoVisibleTargets)
    //    {
    //        if (i.tag == "Rapty")
    //        {
    //            ankyFlee.target = i.gameObject;
    //            Flee.RaptyInSight = RaptyInSight;
    //            if (!ankyFlee.enabled)
    //            ankyFlee.enabled = true;
    //       }




    //    }
    //}

    //GameObject closestRapty = new GameObject();
    //public GameObject closestRapty;
    //void AnkyFlee()
    //{

    //    closestRapty = new GameObject();



    //    foreach (Transform x in viewRapty)
    //    {

    //        Vector3 Difference = new Vector3();
    //        Vector3 RaptorDiff = new Vector3();


    //        Difference = (this.transform.position - x.position);
    //        RaptorDiff = (this.transform.position - closestRapty.transform.position);

    //        if (Difference.magnitude < RaptorDiff.magnitude)
    //        {
    //            closestRapty = x.gameObject;
    //        }
    //        float Distance = Vector3.Distance(x.position, this.transform.position);
    //        if (Distance > 50)
    //        {
    //            viewRapty.Clear();
    //            currentState = ankyState.ALERTED;
    //        }
    //    }

    //    if (closestRapty)
    //    {
    //        ankyFlee.target = closestRapty;
    //    }

    //}
}

