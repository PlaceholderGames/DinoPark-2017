using System.Collections;
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
        _owner.wanderBehaviourScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Alert State");
        _owner.anim.SetBool("isAlerted", false);
        _owner.wanderBehaviourScript.enabled = false;

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


        //Fleeing
        if (_owner.closestHazardDist <= _owner.fleeDistance  || _owner.myStats.health < 10)
        {
            _owner.stateMachine.ChangeState(FleeState.Instance);
        }
        //Attacking
        else if (_owner.predatorsInRange.Count > 0)
        {
            //If we are being attacked and can see our target
            if (_owner.closestHazardDist < _owner.attackRange)
            {
                if (_owner.myStats.health <= 100 && _owner.myStats.health >= 30)
                {
                    _owner.stateMachine.ChangeState(AttackingState.Instance);
                }
            }
        }
        //Grazing
        else if (_owner.predatorsInRange.Count <= 0)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
        }
        //Eating
        else if (_owner.myStats.hunger < 30)
        {
            _owner.stateMachine.ChangeState(EatingState.Instance);
        }
        //Drinking
        else if (_owner.myStats.thirst < 30)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
        }

        _owner.herd();
    }

}


public class RAlertState : State<MyRapty>
{
    private static RAlertState _instance;


    private RAlertState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static RAlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RAlertState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("Entering Alert State");

    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Alert State");
      

    }

    public override void UpdateState(MyRapty _owner)
    {

    }  
}