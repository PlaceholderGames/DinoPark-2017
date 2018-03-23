using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoStateMachine;
using System;



/// Alert State
/// 
/// 





public class AlertState : State<MyAnky>
{
    private static AlertState _instance;




    private AlertState()
    {
      

        if (_instance != null)
        {


            return;

        }
        _instance = this;
    }


    public static AlertState Instance
    {

        get
        {

            if (_instance == null)
            {
                new AlertState();
            }
            return _instance;
        }




    }




    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Enter Alert State");
        _owner.anim.SetBool("isAlert", true);
       

    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit Alert State");
        _owner.anim.SetBool("isAlert", false);
        _owner.fleeScript.enabled = false;
        _owner.WanderScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {
      

       if (_owner.Distance.magnitude >= 50)

        {
            _owner.stateMachine.ChangeDinoState(GrazingState.Instance);
            _owner.fleeScript.enabled = false;
            _owner.WanderScript.enabled = true;

        }


        if (_owner.Distance.magnitude < 50 && _owner.Distance.magnitude > 30)
        {

            _owner.stateMachine.ChangeDinoState(FleeingState.Instance);
            _owner.WanderScript.enabled = false;
            _owner.fleeScript.enabled = true;
        }


        

    }








}

