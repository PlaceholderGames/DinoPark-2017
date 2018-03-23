using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class Drink : State<MyAnky>
{
    private static Drink _instance;

    private Drink()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static Drink Instance
    {
        get
        {
            if (_instance == null)
            {
                new Drink();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Drinking State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Leaving Drinking State");
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.currentState == MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(Grazing.Instance);
        }
        else
        {
            _owner.drinking();
        }       
    }
}
