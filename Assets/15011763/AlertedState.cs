using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
using System;

public class AlertedState : State<MyAnky>
{
    private static AlertedState _Instance;

    private AlertedState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static AlertedState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new AlertedState();
            }
            return _Instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering AlertedState state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting AlertedState state");
    }

    public override void UpdateState(MyAnky _Owner)
    {

        // Check if there is an enemy and at what distance away it is
        // if its within range, change to flee
        // if there is no enemy, change to grazing
        if (_Owner.enemyDistance > 0 && _Owner.enemyDistance < 105  && _Owner.enemyDino != null)
        {
            _Owner.myStateMachine.ChangeState(FleeingState.Instance);
        }
        else if (_Owner.enemyDino == null)
        {
            _Owner.myAnkyFlee.enabled = false;
            _Owner.myStateMachine.ChangeState(GrazingState.Instance);
        }

        // Check if hungryDino is 0, if it is then change to deadState
        if (_Owner.healthyDino <= 0)
        {
            _Owner.myStateMachine.ChangeState(DeadState.Instance);
        }

    }

   
}
