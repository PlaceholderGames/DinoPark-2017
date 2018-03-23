using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine2;
using System;



public class eatingState : State<MyAnky>
{
    private static eatingState _instance;

    private eatingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static eatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new eatingState();
            }
            return _instance;
        }
    }

    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Entered eating state");
        _owner.anim.SetBool("isEating", true);
        _owner.seekScript.enabled = true;
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit eating state");
        _owner.anim.SetBool("isEating", false);
        _owner.seekScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {
        if (_owner.ankyAppetite >= 100)
        {
            _owner.stateMachine.changeState(grazingState.Instance);
        }

        else if (_owner.transform.position.y > 76)
        {
            _owner.seekScript.enabled = false;
            _owner.ankyAppetite += (Time.deltaTime * 5) * 1;
        }
    }

}

