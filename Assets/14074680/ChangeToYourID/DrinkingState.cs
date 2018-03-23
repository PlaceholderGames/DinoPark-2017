using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateDino;
using System;

public class DrinkingState : State <MyAnky> {

    private static DrinkingState _instance;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static DrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Drinking State");
        _owner.AnkySeek.enabled = true;
        _owner.AnkyWander.enabled = false;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Drinking State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.transform.position.y <= 36)
        {
            _owner.AnkySeek.enabled = false;
            _owner.hydration++;
            
        }
        if (_owner.hydration >= 100)
        {
            _owner.AnkyWander.enabled = true;
            _owner.StateMachine.ChangeState(Grasingstate.Instance);
        }
    }
}
