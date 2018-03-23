using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoStateMachine;
using System;



/// 1st State
/// 
/// 





public class Anky_Eating : State<MyAnky>
{
    private static Anky_Eating _instance;




    private Anky_Eating()
    {


        if (_instance != null)
        {


            return;

        }
        _instance = this;
    }


    public static Anky_Eating Instance
    {

        get
        {

            if (_instance == null)
            {
                new Anky_Eating();
            }
            return _instance;
        }




    }




    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Enter Eating State");
        _owner.anim.SetBool("isEating", true);
        _owner.SeekScript.enabled = true;
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit Eating State");
        _owner.anim.SetBool("isEating", false);
        _owner.SeekScript.enabled = false;
        //_owner.WanderScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {
      

        if (_owner.Anky_Food_Level >=100)
        {
            _owner.stateMachine.ChangeDinoState(GrazingState.Instance);

        }



        if (_owner.transform.position.y > 52)
        {

            _owner.SeekScript.enabled = false;
            _owner.Anky_Food_Level += (Time.deltaTime * 5) * 1;

        }


        


    }


}

