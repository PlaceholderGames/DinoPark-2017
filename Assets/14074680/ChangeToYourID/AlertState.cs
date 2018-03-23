using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateDino;
using System;

public class AlertState : State<MyAnky>
{

    private static AlertState _instance;

    private AlertState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static AlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AlertState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _o)
    {
        Debug.Log("Entering Alert State");

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Alert State");
    }

    public override void UpdateState(MyAnky _owner)
    {

    }
}