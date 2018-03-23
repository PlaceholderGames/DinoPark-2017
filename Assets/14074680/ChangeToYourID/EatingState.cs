using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateDino;
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
        Debug.Log("Entering Eating State");
        _owner.AnkyFood.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Eating State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.transform.position.y >= 57)
        {
            _owner.AnkyFood.enabled = false;
            _owner.food++;

        }
        if (_owner.food >= 100)
        {
            _owner.AnkyWander.enabled = true;
            _owner.StateMachine.ChangeState(Grasingstate.Instance);
        }
    }
}
