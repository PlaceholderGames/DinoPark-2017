using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class EatingState : State<MyAnky>
{
    private static EatingState _instance;
    private float timeToEat = 2;
    public float timeToWait = 2;

    private EatingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Eating State");
        _owner.anim.SetBool("isEating", true);
        _owner.setCurrentState(MyAnky.ankyState.EATING);
        _owner.wanderBehaviourScript.enabled = false;
        timeToEat = 0;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Eating State");
        _owner.anim.SetBool("isEating", false);
        timeToEat = 0;
    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.seekBehaviourScript.enabled = false; 
        if (_owner.myStats.health <= 0)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
        else if (timeToEat <= 0)
        {
            //Eat   
            _owner.myStats.hunger += _owner.eat;
            timeToEat = timeToWait;
        }
        timeToEat -= Time.deltaTime;

        //If we still need to eat
        if (_owner.myStats.hunger <= 100)
        {
            //Alert
            if (_owner.predatorsInRange.Count > 0)
            {
                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
        }
        else if (_owner.myStats.hunger >= 100)
        {
            _owner.myStats.hunger = 100;
            _owner.stateMachine.ChangeState(GrazingState.Instance);
        }
    }
}
