using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class Idle : State<MyAnky>
{
    private static Idle _Instance;

    private Idle()
    {
        if(_Instance != null)
        {
            return;
        }

        _Instance = this;
    }

    public static Idle Instance
    {
        get
        {
            if(_Instance == null)
            {
                new Idle();
            }
            return _Instance;
        }
    }


    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Idle state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Idle state");
    }

    public override void UpdateState(MyAnky _Owner)
    {
        //Can only switch to grazing
        _Owner.stateMachine.ChangeState(Grazing.Instance);
    }
}
