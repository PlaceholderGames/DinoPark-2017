using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateDino;
using System;

public class Grasingstate : State<MyAnky>
{

    private static Grasingstate _instance;

    private Grasingstate()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static Grasingstate Instance
    {
        get
        {
            if (_instance == null)
            {
                new Grasingstate();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.AnkyWander.enabled = true;
        Debug.Log("Entering Grasing State");
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.AnkyWander.enabled = false;
        Debug.Log("Exiting Grasing State");
    }

    public override void UpdateState(MyAnky _owner)
    {//Drinking
        if (_owner.hydration < 50)
        {
            _owner.StateMachine.ChangeState(DrinkingState.Instance);
        }
        //Eating
        if (_owner.food < 50)
        {
            _owner.StateMachine.ChangeState(EatingState.Instance);
        }

        //Flee
        if (_owner.AnkyVillains.Count > 0)
        {
            foreach (Transform j in _owner.AnkyVillains)
            {
                float Distanceenemy = Vector3.Distance(j.position, _owner.transform.position);//find closest here
                if (Distanceenemy < 50)
                {
                    _owner.StateMachine.ChangeState(FleeingState.Instance);
                }

            }
        }
    }
}
