using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoStateMachine;
using System;



//Second state


public class Drinking_State : State<MyAnky>
{
    private static Drinking_State _instance;




    private Drinking_State()
    {


        if (_instance != null)
        {


            return;

        }
        _instance = this;
    }


    public static Drinking_State Instance
    {

        get
        {

            if (_instance == null)
            {
                new Drinking_State();
            }
            return _instance;
        }




    }




    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Enter Drinking State");
        _owner.anim.SetBool("isDrinking", true);
        _owner.SeekScript.enabled = true;
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit Drinking State");
        _owner.anim.SetBool("isDrinking", false);
        _owner.SeekScript.enabled = false;
        _owner.WanderScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {
       

        if (_owner.Anky_Water >= 100)
        {
            _owner.stateMachine.ChangeDinoState(GrazingState.Instance);

        }


        if (_owner.transform.position.y <= 36)
        {
           
                _owner.SeekScript.enabled = false;
                _owner.Anky_Water+=(Time.deltaTime *5)*1;
          

            
        }




        /*
        if (_owner.Health_Anky <= 0) {


            _owner.stateMachine.ChangeDinoState(DeadState.Instance);


        }
        */


    }








}

