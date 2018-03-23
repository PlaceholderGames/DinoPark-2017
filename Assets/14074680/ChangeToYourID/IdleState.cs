using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateDino;
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

    public override void EnterState(MyAnky _o)
    {

        Debug.Log("Entering Idle State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (!_owner.switchState)
        {
            {
                _owner.StateMachine.ChangeState(Grasingstate.Instance);
            }
        }
    }
}
