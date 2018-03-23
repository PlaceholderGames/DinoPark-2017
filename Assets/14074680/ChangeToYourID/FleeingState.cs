using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateDino;
using System;

public class FleeingState : State<MyAnky>
{

    private static FleeingState _instance;

    private FleeingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static FleeingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _o)
    {
        Debug.Log("Entering Fleeing State");
        _o.AnkyFlee.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Fleeing State");
        _owner.AnkyFlee.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.AnkyVillains.Count > 0)
        {
            foreach (Transform j in _owner.AnkyVillains)
            {
                float Distanceenemy = Vector3.Distance(j.position, _owner.transform.position);//find closest here
                if (Distanceenemy < 50)
                {
                    _owner.AnkyFlee.target = j.gameObject;
                }
                if (Distanceenemy > 60)
                {
                    _owner.StateMachine.ChangeState(Grasingstate.Instance);
                }
            }
        }
   

    }
}
