using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoStateMachine;
using System;



/// Grazing State
/// 
/// 





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




    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Enter Grazing State");
        _owner.anim.SetBool("isGrazing", true);
        //_owner.SeekScript.enabled = false;
        _owner.WanderScript.enabled = true;


    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit Grazing State");
        _owner.anim.SetBool("isGrazing", false);
        _owner.WanderScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {
   

        if (_owner.Anky_Food_Level <= 80)
        {
            _owner.stateMachine.ChangeDinoState(Anky_Eating.Instance);

        }



        if (_owner.Anky_Water < 60) {

            _owner.stateMachine.ChangeDinoState(Drinking_State.Instance);




        }



        if (_owner.RaptorsInView.Count > 0)
        {
         
           foreach (Transform i in _owner.FieldOV.visibleTargets)
           {


                float AnkyDistance = Vector3.Distance(i.position, _owner.transform.position);
                if (AnkyDistance < 50)
                {



                    _owner.stateMachine.ChangeDinoState(FleeingState.Instance);


                }

           }


        }

    }








}

