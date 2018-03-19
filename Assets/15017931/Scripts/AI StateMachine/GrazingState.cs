using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class GrazingState : State<MyAnky>
{
    private static GrazingState _instance;

    private GrazingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static GrazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Grazing State");
        _owner.anim.SetBool("isGrazing", true);
        _owner.setCurrentState(MyAnky.ankyState.GRAZING);

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Grazing State");
        _owner.anim.SetBool("isGrazing", false);
    }

    public override void UpdateState(MyAnky _owner)
    {

        //Drinking
        //_owner.stateMachine.ChangeState(DrinkingState.Instance);
        //Eating
        //_owner.stateMachine.ChangeState(EatingState.Instance);
        //Alert
        _owner.stateMachine.ChangeState(AlertState.Instance);
        //Idle
        //_owner.stateMachine.ChangeState(IdleState.Instance);
    }
}
