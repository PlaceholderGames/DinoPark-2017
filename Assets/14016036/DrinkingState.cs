using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class DrinkingState : State<MyAnky>
{
    private static DrinkingState _instance;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }


        _instance = this;
    }

    public static DrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }
            return _instance;
        }
    }


    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Drinking State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Drinking State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.anim.GetBool("isGrazing") == true)
        {
            _owner.anky = 1;
        }
        _owner.anim.SetBool("isDrinking", true);
        if (_owner.anim.GetBool("isDrinking") == true && _owner.transform.position.y > 38.5)
        {
            _owner.anim.SetBool("isIdle", false);
            _owner.anim.SetBool("isEating", false);
            _owner.anim.SetBool("isAlerted", false);
            _owner.anim.SetBool("isGrazing", false);
            _owner.anim.SetBool("isAttacking", false);
            _owner.anim.SetBool("isFleeing", false);
            _owner.anim.SetBool("isDead", false);

            _owner.AStarTarget.transform.position = _owner.waterLocation();
            _owner.AStarSearch.target = _owner.AStarTarget;

            if(_owner.PathFollowerO.path == null || _owner.PathFollowerO.path.nodes.Count > 1)
            {
                _owner.PathFollowerO.path = _owner.AStarSearch.path;
            }

            _owner.move(_owner.PathFollowerO.getDirectionVector());
        }
        else if (_owner.transform.position.y <= 38.5)
        {

            //IN HERE INCREMENT WATER VALUE AS YOU ARE IN WATER
            // THEN HAVE EXIT CONDITION BASED ON WATER LEVEL
            _owner.water = _owner.water + 20;
            _owner.health = _owner.health + 20;
            _owner.animClip.Play("Ank_eat");
            if (_owner.anky == 1)
            {
                _owner.anim.SetBool("isGrazing", true);
                _owner.stateMachine.ChangeState(WanderingState.Instance);
                _owner.anim.SetBool("isAlerted", false);
                _owner.anim.SetBool("isDrinking", false);
                _owner.anim.SetBool("isGrazing", true);
                _owner.anim.SetBool("isIdle", false);
                _owner.anim.SetBool("isEating", false);
                _owner.anim.SetBool("isAttacking", false);
                _owner.anim.SetBool("isFleeing", false);
                _owner.anim.SetBool("isDead", false);

            }
            else
            {
                _owner.anim.SetBool("isAlerted", true);
                _owner.stateMachine.ChangeState(AlertState.Instance);
                _owner.anim.SetBool("isAlerted", true);
                _owner.anim.SetBool("isDrinking", false);
                _owner.anim.SetBool("isGrazing", false);
                _owner.anim.SetBool("isIdle", false);
                _owner.anim.SetBool("isEating", false);
                _owner.anim.SetBool("isAttacking", false);
                _owner.anim.SetBool("isFleeing", false);
                _owner.anim.SetBool("isDead", false);

            }
        }
    }
}