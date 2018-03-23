using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class MyAnky : Agent
{
    public enum AnkyState
    {
        IDLE,       
        EATING,     
        DRINKING,  
        ALERTED,      
        GRAZING,    
        ATTACKING,  
        FLEEING,     
        DEAD
    };

    public Animator anim;
    public AnkyState currentState;
    public AnkyState previousState;
    public Wander wander;
    public Flee run;
    public Seek seek;
    public Face face;
    public FieldOfView vision;
    public GameObject water;
    public GameObject food;
    public GameObject raptor;
    private float raptorDistance;
    private float dehydrated;
    private float hungry;


    // Use this for initialization
    protected override void Start()
    {
        wander = GetComponent<Wander>();
        run = GetComponent<Flee>();
        seek = GetComponent<Seek>();
        vision = GetComponent<FieldOfView>();
        face = GetComponent<Face>();
        anim = GetComponent<Animator>();
        water = GameObject.Find("Daylight Water");
        food = GameObject.Find("Terrain");
        raptor = GameObject.Find("Rapty");
        raptorDistance = 100;
        dehydrated = 200;
        hungry = 50;
        currentState = AnkyState.IDLE;
        previousState = 0;

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
        //Debug.Log("Value of dehydrated");
        //Debug.Log(dehydrated);
        dehydrated -= Time.deltaTime;
        //Debug.Log("Value of hungry");
        //Debug.Log(hungry);
        hungry -= Time.deltaTime;

        for (int i = 0; i < vision.visibleTargets.Count; i++) //cycles throught the list of what the anky can see
        {
            if (vision.visibleTargets[i].transform.name == "Rapty")
            {
                raptor = vision.visibleTargets[i].gameObject;
            }
            if (vision.visibleTargets[i].transform.name == "Anky")
            {
                currentState = AnkyState.GRAZING;
            }
        }

        if (Vector3.Distance(raptor.transform.position, transform.position) < raptorDistance)
        {
            currentState = AnkyState.ALERTED;
        }
        else
        {
            currentState = AnkyState.GRAZING;
        }

        if (currentState != AnkyState.EATING)
        {
            currentState = AnkyState.GRAZING;
        }

            switch (currentState)
        {
            case AnkyState.EATING:
                EATING();
                break;
            case AnkyState.DRINKING:
                DRINKING();
                break;
            case AnkyState.ALERTED:
                ALERTED();
                break;
            case AnkyState.GRAZING:
                GRAZING();
                break;
            case AnkyState.ATTACKING:
                ATTACKING();
                break;
            case AnkyState.FLEEING:
                FLEEING();
                break;
            case AnkyState.DEAD:
                DEAD();
                break;
        }

        base.Update();
       
    }

    void setAnim()
    {
        switch (previousState)
        {
            case AnkyState.IDLE:
                anim.SetBool("isIdle", false);
                break;
            case AnkyState.EATING:
                anim.SetBool("isEating", false);
                break;
            case AnkyState.DRINKING:
                seek.enabled = false;
                anim.SetBool("isDrinking", false);
                break;
            case AnkyState.ALERTED:
                face.enabled = false;
                anim.SetBool("isAlerted", false);
                break;
            case AnkyState.GRAZING:
                wander.enabled = false;
                anim.SetBool("isGrazing", false);
                break;
            case AnkyState.ATTACKING:
                anim.SetBool("isAttacking", false);
                break;
            case AnkyState.FLEEING:
                run.enabled = false;
                anim.SetBool("isFleeing", false);
                break;
            case AnkyState.DEAD:
                anim.SetBool("isDead", false);
                break;
        }
    }

    void switchStates(AnkyState newState)
    {
       if(currentState != newState)
        {
            previousState = currentState;
            currentState = newState;
        }
    }

    void EATING()
    {
        Debug.Log("Hello is it me your looking for state 1");
        Debug.Log("Value of hungry in Eating");
        Debug.Log(hungry);
        wander.enabled = false;
        seek.target = food;
        seek.enabled = true;
        hungry += Time.deltaTime * 100;

        if (hungry >= 100)
        {
            switchStates(AnkyState.GRAZING);
        }
        else if (hungry <= 0)
        {
            switchStates(AnkyState.DEAD);
        }
    }

    void DRINKING()
    {
        Debug.Log("Hello is it me your looking for state 2");
        Debug.Log("Value of hungry in Drinking");
        Debug.Log(dehydrated);
        wander.enabled = false;
        seek.target = water;
        seek.enabled = true;
        dehydrated += Time.deltaTime * 100;

        if (dehydrated >= 200)
        {
            switchStates(AnkyState.GRAZING);
        }
        else if (dehydrated <= 0)
        {
            switchStates(AnkyState.DEAD);
        }
    }

    void ALERTED()
    {
        Debug.Log("Hello is it me your looking for state 3");
        face.target = raptor;
        face.enabled = true;
    }

    void GRAZING()
    {
        Debug.Log("Hello is it me your looking for state 4");
        wander.enabled = true;
        if (hungry <= 20)
        {
            wander.enabled = false;
            switchStates(AnkyState.EATING);
        }
        if (dehydrated <= 20)
        {
            wander.enabled = false;
            switchStates(AnkyState.DRINKING);
        }
    }

    void ATTACKING()
    {
        
    }

    void FLEEING()
    {
        Debug.Log("Hello is it me your looking for state 6");
        run.target = raptor;
        run.enabled = true;
    }

    void DEAD()
    {
        Debug.Log("Hello is it me your looking for state 7");
        wander.enabled = false;
        run.enabled = false;
        face.enabled = false;
        seek.enabled = false;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
