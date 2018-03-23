using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoStateMachine;
using System;



/// 1st State
/// 
/// 





public class AttackState : State<MyAnky>
{
    private static AttackState _instance;
    int Health_Anky = 100;
    int Anky_Water = 100;
    public float HealthTimer;


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




    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Enter Attack State");
        _owner.anim.SetBool("Attack", true);
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit Attack State");
        _owner.anim.SetBool("Attack", false);
    }

    public override void updateState(MyAnky _owner)
    {

         



 /*if (Health_Anky <= 0)
          {


              Debug.Log("Dead");
              _owner.stateMachine.ChangeDinoState(DeadState.Instance);

          }


    */


       

        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeDinoState(FleeingState.Instance);

        }





         
          



    }








}

