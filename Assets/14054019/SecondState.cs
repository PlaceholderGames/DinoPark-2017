using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class SecondState : State<MyAnky>
{
    private static SecondState _instance;
    private object _Owner;

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
            if (_instance == null)
            {
                new SecondState();
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
        Debug.Log("Exiting Drinking State");
    }

    public override void UpdateState(MyAnky _owner)
    {

    }
}

