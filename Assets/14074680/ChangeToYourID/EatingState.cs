using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateDino;
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

    public override void EnterState(MyAnky _o)
    {
        Debug.Log("Entering Eating State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Eating State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (!_owner.switchState)
        {
            _owner.StateMachine.ChangeState(DrinkingState.Instance);
        }
    }
}
