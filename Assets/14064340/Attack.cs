﻿using UnityEngine;
using Statestuff;

public class AttackState : State<MyAnky>
{
    private static AttackState _instance;

    private AttackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static AttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAttacking", true);
        Debug.Log("entering Attack state");
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAttacking", false);
        Debug.Log("exiting AttackState");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
    }
}
