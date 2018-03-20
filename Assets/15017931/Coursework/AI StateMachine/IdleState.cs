using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class IdleState : State<MyAnky>
{
    private static IdleState _instance;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Idle State");
        _owner.anim.SetBool("isIdle", true);
        _owner.setCurrentState(MyAnky.ankyState.GRAZING);
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Idle State");
        _owner.anim.SetBool("isIdle", false);
    }

    public override void UpdateState(MyAnky _owner)
    {
        //Grazing 
        _owner.stateMachine.ChangeState(GrazingState.Instance);
    }
}
