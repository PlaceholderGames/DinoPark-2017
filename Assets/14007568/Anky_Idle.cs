using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoStateMachine;
using System;



/// Idle State
/// 
/// 





public class IdleState : State<MyAnky>
{
    private static IdleState _instance;




    private IdleState()
    {


        if (_instance != null)
        {


            return;

        }
        _instance = this;
    }


    public static IdleState Instance
    {

        get
        {

            if (_instance == null)
            {
                new IdleState();
            }
            return _instance;
        }




    }




    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Enter Idle State");
        _owner.anim.SetBool("Idle", true);
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit Idle State");
        _owner.anim.SetBool("Idle", false);
    }

    public override void updateState(MyAnky _owner)
    {
        

        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeDinoState(Drinking_State.Instance);

        }


    }








}

