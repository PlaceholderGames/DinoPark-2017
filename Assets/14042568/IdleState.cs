 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateStuff;

public class IdleState : State<MyRapty>
{
    private static IdleState _Instance;
    private IdleState()
    {
        if(_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new IdleState();
            }

            return _Instance;
        }
    }



    public override void enterState(MyRapty _object)
    {
        Debug.Log("Entering Idle State");
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting First State");
    }

    public override void UpdateState(MyRapty _owner)
    {
        Debug.Log("updating Idle State");
        _owner.raptyMachine.ChangeState(SearchingState.Instance);
    }
}
