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
        _owner.fleeBehaviourScript.target = _owner.closestHazard.gameObject;
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
        //Alerted
        //If we can no longer see a predator
        if (_owner.predatorsInRange.Count <= 0)
        {
            waitForSeconds -= Time.deltaTime;
            //if we have waited and there are still no predators in our vision
            if (waitForSeconds <= 0)
            { 
                //check we are not dead
                if (_owner.health <= 0)
                    _owner.stateMachine.ChangeState(DeadState.Instance);
                //Go back to alert when we cant see any predators anymore
                else
                    _owner.stateMachine.ChangeState(AlertState.Instance);
            }
            //Check if we are dead
            else if (_owner.health <= 0)
            {
                _owner.stateMachine.ChangeState(DeadState.Instance);
            }
            //Attacking
            //If we are being attacked and can see our target
            else if (false)
            {
                _owner.stateMachine.ChangeState(AttackingState.Instance);
            }
        }
        else
        {
            waitForSeconds = 2;
        }
    }
}
