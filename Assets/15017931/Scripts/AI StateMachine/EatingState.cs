using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
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
        _owner.anim.SetBool("isEating", true);
        _owner.setCurrentState(MyAnky.ankyState.EATING);
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Eating State");
        _owner.anim.SetBool("isEating", false);
    }

    public override void UpdateState(MyAnky _owner)
    {


    }
}
