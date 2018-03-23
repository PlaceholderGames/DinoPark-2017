using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateStuff;

public class DeadState : State<MyRapty>
{
    private static DeadState _Instance;
    private DeadState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static DeadState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new DeadState();
            }

            return _Instance;
        }
    }



    public override void enterState(MyRapty _owner)
    {
        _owner.myAS_instance.enabled = false;
        _owner.myAS_pathFollower.enabled = false;
        _owner.myA_star.enabled = false;
        _owner.myPursue.enabled = false;
        _owner.myWander.enabled = false;
        _owner.myAgent.enabled = false;
        _owner.transform.Rotate(0.0f, 0.0f, 180.0f);
        Debug.Log("You LOSE! Good DAY Sir!!");
    }

    public override void ExitState(MyRapty _owner)
    {
       
    }

    public override void UpdateState(MyRapty _owner)
    {
    
    }
}