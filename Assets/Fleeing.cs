using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class Fleeing : State<MyAnky>
{
    private static Fleeing _instance;

    private Fleeing()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static Fleeing Instance
    {
        get
        {
            if(_instance == null)
            {
                new Fleeing();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering fleeing State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Leaving fleeing State");
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.currentState == MyAnky.ankyState.ALERTED)
        {
            _owner.stateMachine.ChangeState(Alerted.Instance);
        }

        else
        {
            _owner.fleeing();
        }
    }
}
