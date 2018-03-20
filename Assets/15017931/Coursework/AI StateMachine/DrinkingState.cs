using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class DrinkingState : State<MyAnky>
{
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
        _owner.anim.SetBool("isDrinking", true);
        _owner.setCurrentState(MyAnky.ankyState.DRINKING);
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Dead State");
        _owner.anim.SetBool("isDrinking", false);
    }

    public override void UpdateState(MyAnky _owner)
    {


    }
}
