using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class Alerted : State<MyAnky>
{
    private static Alerted _instance;

    private Alerted()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static Alerted Instance
    {
        get
        {
            if (_instance == null)
            {
                new Alerted();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Alerted State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Leaving Alerted State");
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.currentState == MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(Grazing.Instance);
        }
        else if (_owner.currentState == MyAnky.ankyState.FLEEING)
        {
            _owner.stateMachine.ChangeState(Fleeing.Instance);
        }
        else
        {
            _owner.alerted();
        }
    }
}
