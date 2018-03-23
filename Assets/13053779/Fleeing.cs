using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine2;
using System;



public class fleeingState : State<MyAnky>
{
    private static fleeingState _instance;

    private fleeingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static fleeingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new fleeingState();
            }
            return _instance;
        }
    }

    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Entered fleeing state");
        _owner.anim.SetBool("isFleeing", true);
        _owner.seekScript.enabled = true;
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit fleeing state");
        _owner.anim.SetBool("isFleeing", false);
        _owner.seekScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {

        _owner.fleeScript.enabled = true;

        if(_owner.raptyInFOV.Count > 0)
        {
            foreach (Transform i in _owner.raptyInFOV)
            {
                float ankyDistance = Vector3.Distance(i.position, _owner.transform.position);
                if(ankyDistance < 35)
                {
                    _owner.fleeScript.target = i.gameObject;
                }
                if(ankyDistance > 70)
                {
                    _owner.stateMachine.changeState(grazingState.Instance);
                }
            }
        }
    }
}

