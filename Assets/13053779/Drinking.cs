using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine2;
using System;



public class drinkingState : State<MyAnky>
    {
    private static drinkingState _instance;

    private drinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static drinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new drinkingState();
            }
            return _instance;
        }
    }

    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Entered drinking state");
        _owner.anim.SetBool("isDrinking", true);
        _owner.seekScript.enabled = true;
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit drinking state");
        _owner.anim.SetBool("isDrinking", false);
        _owner.seekScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {
        if(_owner.ankyHydration >= 100)
        {
            _owner.stateMachine.changeState(grazingState.Instance);
        }

        else if (_owner.transform.position.y < 36)
        {
            _owner.seekScript.enabled = false;
            _owner.ankyHydration += (Time.deltaTime * 5) * 1;
        }
    }

}

