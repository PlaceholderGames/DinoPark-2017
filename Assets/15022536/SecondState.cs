using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class SecondState : State<MyAnky>
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
            // check if the state exist
            if (_instance == null)
            {
                new SecondState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering second State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting second State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (!_owner.switchState == true)
        {
            _owner.stateMachine.ChangeState(SecondState.Instance);
        }
    }
}
