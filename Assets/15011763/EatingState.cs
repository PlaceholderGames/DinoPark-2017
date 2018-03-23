using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
using System;

public class EatingState : State<MyAnky>
{
    private static EatingState _Instance;

    private EatingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static EatingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new EatingState();
            }
            return _Instance;
        }
    }

    IEnumerator EatingFood(MyAnky _Owner)
    {
        yield return new WaitForSeconds(0.5f);
        if (_Owner.hungryDino <= 100)
        {
            _Owner.hungryDino = _Owner.hungryDino + 0.1f;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Eating state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Eating state");
    }

    public override void UpdateState(MyAnky _Owner)
    {
        // if hunger is lower than 100
        // Eat
        // Else go back to grazing
        if (_Owner.hungryDino < 100)
        {
            _Owner.StartCoroutine(EatingFood(_Owner));
        }
        else
        {
            _Owner.myStateMachine.ChangeState(GrazingState.Instance);
        }

        // Kill dino if health is 0
        if (_Owner.healthyDino <= 0)
        {
            _Owner.myStateMachine.ChangeState(DeadState.Instance);
        }

    }
}
