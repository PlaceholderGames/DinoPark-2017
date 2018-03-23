using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateStuff;

public class FleeingState : State<MyRapty>
{
    private static FleeingState _Instance;
    private FleeingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static FleeingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new FleeingState();
            }

            return _Instance;
        }
    }



    public override void enterState(MyRapty _object)
    {
        Debug.Log("Entering Second State");
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Second State");
    }

    public override void UpdateState(MyRapty _owner)
    {
        
    }
}