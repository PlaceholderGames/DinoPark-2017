using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoStateMachine;
using System;



/// Fleeing State
/// 
/// 





public class FleeingState : State<MyAnky>
{
    private static FleeingState _instance;




    private FleeingState()
    {


        if (_instance != null)
        {


            return;

        }
        _instance = this;
    }


    public static FleeingState Instance
    {

        get
        {

            if (_instance == null)
            {
                new FleeingState();
            }
            return _instance;
        }




    }




    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Enter Fleeing State");
        _owner.anim.SetBool("isFleeing", true);
        _owner.fleeScript.enabled = true;
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit Fleeing State");
       _owner.anim.SetBool("isFleeing", false);
        _owner.fleeScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {
        _owner.fleeScript.enabled = true;

        if (_owner.Distance.magnitude > 30)
        {
            _owner.stateMachine.ChangeDinoState(GrazingState.Instance);
            _owner.fleeScript.enabled = false;

        }


    }








}

