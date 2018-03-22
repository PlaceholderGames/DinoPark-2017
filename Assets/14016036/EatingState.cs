using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class EatingState : State<MyAnky>
{
    private static EatingState _instance;

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
        Debug.Log("Entering Fleeing State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Fleeing State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAlerted", false);
        _owner.anim.SetBool("isDrinking", false);
        _owner.anim.SetBool("isGrazing", false);
        _owner.anim.SetBool("isIdle", false);
        _owner.anim.SetBool("isEating", true);
        _owner.anim.SetBool("isAttacking", false);
        _owner.anim.SetBool("isHerding", false);
        _owner.anim.SetBool("isFleeing", false);
        _owner.anim.SetBool("isDead", false);
        _owner.time -= Time.deltaTime;
        if(_owner.time < 0)
        {
            if (_owner.anky == 1)
            {
                _owner.stateMachine.ChangeState(WanderingState.Instance);
            }
            else
            {
                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
        }
    }
}
