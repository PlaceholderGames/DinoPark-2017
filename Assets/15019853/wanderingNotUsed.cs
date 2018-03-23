using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class Wandering : State<MyAnky>
{
    private static Wandering _instance;

    private Wandering()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static Wandering Instance
    {
        get
        {
            if (_instance == null)
            {
                new Wandering();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Wandering State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Leaving Wandering State");
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.currentState==MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(Wandering.Instance);
        }
    }
}
