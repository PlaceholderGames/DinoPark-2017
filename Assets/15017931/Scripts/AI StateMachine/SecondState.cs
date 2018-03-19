using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class SecondState : State<AI>
{
    private static SecondState _instance;


    private SecondState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static SecondState Instance
    {
        get
        {
            if (_instance == null)
            {
                new SecondState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering second State");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting second State");
    }

    public override void UpdateState(AI _owner)
    {
        if (!_owner.switchState)
        {
            _owner.stateMachine.ChangeState(FirstState.Instance);
        }

    }
}
