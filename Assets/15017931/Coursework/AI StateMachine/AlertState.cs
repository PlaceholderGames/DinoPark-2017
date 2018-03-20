﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class AlertState : State<MyAnky>
{
    private static AlertState _instance;


    private AlertState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AlertState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Alert State");
        _owner.anim.SetBool("isAlerted", true);
        _owner.setCurrentState(MyAnky.ankyState.ALERTED);
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Alert State");
        _owner.anim.SetBool("isAlerted", false);

    }

    public override void UpdateState(MyAnky _owner)
    {

        //Check distance between us and our closest hazard
        foreach (Transform pred in _owner.predatorsInRange)
        {
            //Distance between us and predator
            _owner.distance = Vector3.Distance(_owner.transform.position, pred.transform.position);
            //find the closets predator
            if (_owner.distance < _owner.closestHazardDist)
            {
                _owner.closestHazard = pred;
                _owner.closestHazardDist = _owner.distance;
            }
        }


        //Attack
        //_owner.stateMachine.ChangeState(AttackingState.Instance);
        
        //Grazing
        if(_owner.predatorsInRange.Count  <= 0)
            _owner.stateMachine.ChangeState(GrazingState.Instance);

        //Drinking
        //_owner.stateMachine.ChangeState(DrinkingState.Instance);
        //Eating
        //_owner.stateMachine.ChangeState(EatingState.Instance);
        //Fleeing
        else if (_owner.closestHazardDist <= _owner.fleeDistance)
            _owner.stateMachine.ChangeState(FleeState.Instance);

    }
}
