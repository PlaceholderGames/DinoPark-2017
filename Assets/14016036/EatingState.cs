using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class EatingState : State<MyAnky>
{
    private static EatingState _instance;

    private EatingState()
    {
        if (_instance != null)
        {
            return;
        }


        _instance = this;
    }

    public static EatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatingState();
            }
            return _instance;
        }
    }


    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Fleeing State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Fleeing State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        //_owner.ankyTerrain.terrainData.GetDetailLayer(0, 0, 0, 0, 1);
    }
}
