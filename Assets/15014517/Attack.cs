using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class AttackState : State<MyAnky>
{
    private static AttackState _Instance;

    private AttackState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static AttackState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new AttackState();
            }
            return _Instance;
        }
    }


    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Attack state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Attack state");
    }

    public override void UpdateState(MyAnky _Owner)
    {


        if (_Owner.hunger <= 0 && _Owner.thirst <= 0)
        {
            _Owner.stateMachine.ChangeState(DeadState.Instance);
        }

        // can go to attack, flee alert or dead.


    }
}