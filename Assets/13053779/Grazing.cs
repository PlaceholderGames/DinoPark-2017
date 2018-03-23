using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine2;
using System;



public class grazingState : State<MyAnky>
{
    private static grazingState _instance;

    private grazingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static grazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new grazingState();
            }
            return _instance;
        }
    }

    public override void enterState(MyAnky _owner)
    {
        Debug.Log("Entered grazing state");
        _owner.anim.SetBool("isGrazing", true);
        _owner.wanderScript.enabled = true;
    }

    public override void exitState(MyAnky _owner)
    {
        Debug.Log("Exit grazing state");
        _owner.anim.SetBool("isGrazing", false);
        _owner.wanderScript.enabled = false;
    }

    public override void updateState(MyAnky _owner)
    {
        if (_owner.ankyAppetite < 45)
        {
            _owner.stateMachine.changeState(eatingState.Instance);
        }

        if (_owner.ankyHydration < 65)
        {
            _owner.stateMachine.changeState(drinkingState.Instance);
        }

        if (_owner.raptyInFOV.Count > 0)
        {
            foreach (Transform i in _owner.FOV.visibleTargets)
            {
                float ankyDistance = Vector3.Distance(i.position, _owner.transform.position);
                if (ankyDistance < 75)
                {
                    _owner.stateMachine.changeState(fleeingState.Instance);
                }
            }
        }
    }
}