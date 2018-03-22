﻿using UnityEngine;
using Statestuff;

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

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering eating state");
        _owner.currentAnkyState = MyAnky.ankyState.EATING;
        _owner.anim.SetBool("isEating", true);

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting eating state");
        _owner.anim.SetBool("isEating", false);
    }

    public override void UpdateState(MyAnky _owner)
    {

        //if (_owner.asPathFollowerScript.path.nodes.Count < 0 || _owner.asPathFollowerScript.path == null)
        //    _owner.asPathFollowerScript.path = _owner.aStarScript.path;
        //
        //_owner.move(_owner.asPathFollowerScript.getDirectionVector());


        _owner.energy += (Time.deltaTime * 1) * 1;
        

        if (_owner.energy >= 100)
        {
            _owner.stateMachine.ChangeState(grazingState.Instance);
        }

        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(deadState.Instance);
        }

    }
}