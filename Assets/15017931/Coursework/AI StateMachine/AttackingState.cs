using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class AttackingState : State<MyAnky>
{
    private static AttackingState _instance;


    private AttackingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AttackingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Attacking State");
        _owner.anim.SetBool("isAttacking", true);
        _owner.setCurrentState(MyAnky.ankyState.ATTACKING);
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Attacking State");
        _owner.anim.SetBool("isAttacking", false);
    }

    public override void UpdateState(MyAnky _owner)
    {

    


    }
}
