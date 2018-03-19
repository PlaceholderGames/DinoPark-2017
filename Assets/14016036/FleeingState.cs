using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class FleeingState : State<MyAnky>
{
    private static FleeingState _instance;

    private FleeingState()
    {
        if (_instance != null)
        {
            return;
        }


        _instance = this;
    }

    public static FleeingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeingState();
            }
            return _instance;
        }
    }


    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Fleeing State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Fleeing State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        //if (!_owner.switchState)
        //{
        //    _owner.stateMachine.ChangeState(WanderingState.Instance);
        //}

        _owner.currentState = MyAnky.ankyState.FLEEING;
        // _owner.anim.SetTrigger("isFleeing");
        _owner.anim.SetBool("isAlerted", false);
        _owner.anim.SetBool("isDrinking", false);
        _owner.anim.SetBool("isGrazing", false);
        _owner.anim.SetBool("isIdle", false);
        _owner.anim.SetBool("isEating", false);
        _owner.anim.SetBool("isAttacking", false);
        _owner.anim.SetBool("isFleeing", true);
        _owner.anim.SetBool("isDead", false);
        //Debug.Log("its getting in this Alert");
        _owner.ankyFlee.enabled = true;
        _owner.ankyWander.enabled = false;
        _owner.ankyAgent.maxSpeed = 3;

        for (int i = 0; i < _owner.ankyEnemies.Count; i++)
        {
            Transform target = _owner.ankyEnemies[i];
            if (Vector3.Distance(target.position, _owner.transform.position) > 50)
            {
                _owner.ankyAgent.maxSpeed = 1;
                _owner.anim.SetBool("isAlerted", true);
                _owner.anim.SetBool("isDrinking", false);
                _owner.anim.SetBool("isGrazing", false);
                _owner.anim.SetBool("isIdle", false);
                _owner.anim.SetBool("isEating", false);
                _owner.anim.SetBool("isAttacking", false);
                _owner.anim.SetBool("isFleeing", false);
                _owner.anim.SetBool("isDead", false);
                _owner.stateMachine.ChangeState(AlertState.Instance);
                
            }
        }

    }
}