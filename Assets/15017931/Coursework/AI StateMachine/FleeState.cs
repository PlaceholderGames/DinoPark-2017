using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;



public class FleeState : State<MyAnky>
{
    private static FleeState _instance;
    private float waitForSeconds = 2;

    private FleeState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Flee State");
        _owner.anim.SetBool("isFleeing", true);
        _owner.anim.SetBool("isAlerted", true);
        _owner.setCurrentState(MyAnky.ankyState.FLEEING);
        _owner.fleeBehaviourScript.target = _owner.myTarget.gameObject;
        _owner.fleeBehaviourScript.enabled = true;
        waitForSeconds = 2;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Flee State");
        _owner.anim.SetBool("isFleeing", false);
        _owner.fleeBehaviourScript.enabled = false;
        waitForSeconds = 2;
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
        //Alerted       
        //Check if we are dead
        if (_owner.myStats.health <= 0)
        {
            _owner.anim.SetBool("isAlerted", false);
            _owner.anim.SetBool("isDead", true);
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }
        //If we can no longer see a predator
        else if (_owner.predatorsInRange.Count <= 0)
        {
            waitForSeconds -= Time.deltaTime;
            //if we have waited and there are still no predators in our vision
            if (waitForSeconds <= 0)
            {
                //Go back to alert when we cant see any predators anymore
                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
        }
        else if (_owner.predatorsInRange.Count > 0)
        {          
            //Attacking
            //If we are being attacked and can see our target
            if (_owner.closestHazardDist < _owner.attackRange)
            {
                if (_owner.myStats.health <= 100 && _owner.myStats.health >= 30)
                {
                    _owner.stateMachine.ChangeState(AttackingState.Instance);
                }
            }
            //Remain in our currnet state
            waitForSeconds = 2;
        }
    }
}

