using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

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
            // check if the state exist
            if(_instance == null)
            {
                new FirstState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering first State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting first State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if(_owner.switchState == true)
        {
            _owner.stateMachine.ChangeState(SecondState.Instance);
        }
    }
}
