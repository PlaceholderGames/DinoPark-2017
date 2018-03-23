using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class Grazing : State<MyAnky>
{
    private static Grazing _instance;

    private Grazing()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static Grazing Instance
    {
        get
        {
            if (_instance == null)
            {
                new Grazing();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Grazing State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Leaving Grazing State");
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.currentState == MyAnky.ankyState.ALERTED)
        {
            _owner.stateMachine.ChangeState(Alerted.Instance);
        }
        else if (_owner.currentState == MyAnky.ankyState.DRINKING)
        {
            _owner.stateMachine.ChangeState(Drink.Instance);
        }
        else if (_owner.currentState == MyAnky.ankyState.FLEEING)
        {
            _owner.stateMachine.ChangeState(Fleeing.Instance);
        }
        else
        {
            _owner.grazing();
        }
    }
}
