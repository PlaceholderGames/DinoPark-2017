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
        if (_owner.anim.GetBool("isDrinking") == true && _owner.transform.position.y < 35.5)
        {
            _owner.anim.SetBool("isIdle", false);
            _owner.anim.SetBool("isEating", false);
            _owner.anim.SetBool("isAlerted", false);
            _owner.anim.SetBool("isGrazing", false);
            _owner.anim.SetBool("isAttacking", false);
            _owner.anim.SetBool("isFleeing", false);
            _owner.anim.SetBool("isDead", false);
            //Debug.Log("its getting in this Drink");
            //StartCoroutine(myCoroutine());
            _owner.health = 100;
        }
        else
        {
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