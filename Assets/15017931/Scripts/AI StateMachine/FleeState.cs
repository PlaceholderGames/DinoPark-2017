using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class FleeState : State<MyAnky>
{
    private static FleeState _instance;


    private FleeState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Flee State");
        _owner.anim.SetBool("isFleeing", true);
        _owner.setCurrentState(MyAnky.ankyState.FLEEING);
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Flee State");
        _owner.anim.SetBool("isFleeing", false);
    }

    public override void UpdateState(MyAnky _owner)
    {

        //Alert
        _owner.stateMachine.ChangeState(AlertState.Instance);
      
        
        
        //Attacking
        // _owner.stateMachine.ChangeState(AttackingState.Instance);
        //Dead 
        //_owner.stateMachine.ChangeState(DeadState.Instance);



    }
}
