﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class DrinkingState : State<MyAnky>
{
    private static DrinkingState _instance;
    private float timeToDrink = 2;
    public float timeToWait = 2;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static DrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Drinking State");
        _owner.anim.SetBool("isDrinking", true);
        _owner.setCurrentState(MyAnky.ankyState.DRINKING);
        timeToDrink = 0;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Drinking State");
        _owner.anim.SetBool("isDrinking", false);
        timeToDrink = 0;
    }

    public override void UpdateState(MyAnky _owner)
    {

        //If we should still drink
        if (_owner.myStats.thirst < 100)
        {
            //If we arent on water, go to water
            if (_owner.gameObject.transform.position.y > 36)
            {
                //Use A* to find closest point on map that is at y 36 or less (water level)
            }
            else
            {
                //We can only gain our thirst back after n... seconds
                if (timeToDrink <= 0)
                {
                    //Drink
                    _owner.myStats.thirst += _owner.drink;
                    timeToDrink = timeToWait;
                    Debug.Log("Drank");
                }
                timeToDrink -= Time.deltaTime;
                Debug.Log(timeToDrink);
            }
        }

        //Ensure we cannot go over 100
        if(_owner.myStats.thirst > 100)
        {
            _owner.myStats.thirst = 100;
        }
        
        //Alert
        if (_owner.predatorsInRange.Count > 0)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
    }
}
