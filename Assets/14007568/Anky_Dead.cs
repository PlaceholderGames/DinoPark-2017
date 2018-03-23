using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoStateMachine;
using System;



/// Alert State
/// 
/// 





public class DeadState : State<MyAnky>
{
    private static DeadState _instance;




    private DeadState()
    {


        if (_instance != null)
        {


            return;

        }
        _instance = this;
    }


    public static DeadState Instance
    {

        get
        {

            if (_instance == null)
            {
                new DeadState();
            }
            return _instance;
        }




    }




    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Enter Dead State");
        _owner.anim.SetBool("Dead", true);
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit Dead State");
        _owner.anim.SetBool("Dead", false);
    }

    public override void updateState(MyAnky _owner)
    {
       

    }








}

