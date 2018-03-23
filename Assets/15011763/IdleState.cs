using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
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

        _Owner.myAnkyWander.enabled = true;
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Idle state");
    }

    public override void UpdateState(MyAnky _Owner)
    {
        // Switch to Grazing
        _Owner.myStateMachine.ChangeState(GrazingState.Instance);

        // Kill dino if health is 0
        if (_Owner.hungryDino <= 0)
        {
            _Owner.myStateMachine.ChangeState(DeadState.Instance);
        }
    }


}
