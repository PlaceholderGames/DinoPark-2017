using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class FirstState : State<MyAnky>
{
    private static FirstState _instance;

    private FirstState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FirstState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FirstState();

            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering IDLE State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting IDLE State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(SecondState.Instance);
        }
    }
}


