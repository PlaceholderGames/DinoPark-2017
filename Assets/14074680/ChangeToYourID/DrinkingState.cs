using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateDino;
using System;

public class DrinkingState : State <MyAnky> {

    private static DrinkingState _instance;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static DrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _o)
    {
        Debug.Log("Entering Drinking State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Drinking State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if( _owner.switchState)
        {
            _owner.StateMachine.ChangeState(EatingState.Instance);
        }
    }
}
