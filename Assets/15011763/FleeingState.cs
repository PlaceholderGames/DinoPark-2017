using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
using System;

public class FleeingState : State<MyAnky>
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

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Fleeing state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Fleeing state");
    }

    public override void UpdateState(MyAnky _Owner)
    {
        // Check if there is an enemy and the distance of the enemy
        // if the distance of the enemy is between a certain distance, then flee
        // if there is no dino, then go back to alerted
        if (_Owner.enemyDistance > 0 && _Owner.enemyDistance < 105 && _Owner.enemyDino != null)
        {
            _Owner.myAnkyFlee.target = _Owner.enemyDino;
            _Owner.myAnkyFlee.enabled = true;
        }
        else if (_Owner.enemyDistance < 0 && _Owner.enemyDino == null)
        {
            _Owner.myAnkyFlee.enabled = false;
            _Owner.myStateMachine.ChangeState(AlertedState.Instance);
        }

        // Kill dino if health is 0
        if (_Owner.healthyDino <= 0)
        {
            _Owner.myStateMachine.ChangeState(DeadState.Instance);
        }
    }
}
