using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
using System;

public class HerdingState : State<MyAnky>
{
    private static HerdingState _Instance;

    private HerdingState()
    {
        if (_Instance != null)
        {
            return;
        }

        _Instance = this;
    }
    public static HerdingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new HerdingState();
            }
            return _Instance;
        }
    }
    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering herding state");
    }
    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting herding state");
        _Owner.myAnkyHerd.enabled = false;

    }
    public override void UpdateState(MyAnky _Owner)
    {
        _Owner.myAnkyHerd.target = _Owner.teamDino;

        // If team member is outside 60 distance
        // Start Herding
        // Else go back to grazing
        if (_Owner.teamDistance > 60)
        {

            _Owner.myAnkyHerd.enabled = true;
        }
        else if (_Owner.teamDistance < 40)
        {
            
            _Owner.myStateMachine.ChangeState(GrazingState.Instance);
            Debug.Log("AM I HERE");
        }

        // Kill Dino if health is 0
        if (_Owner.healthyDino <= 0)
        {
            _Owner.myStateMachine.ChangeState(DeadState.Instance);
        }

    }
}
