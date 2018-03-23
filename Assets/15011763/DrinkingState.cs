using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
using System;

public class DrinkingState : State<MyAnky>
{
    private static DrinkingState _Instance;

    private DrinkingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static DrinkingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new DrinkingState();
            }
            return _Instance;
        }
    }

    IEnumerator DrinkingWater(MyAnky _Owner)
    {
        yield return new WaitForSeconds(0.5f);
        if (_Owner.thirstyDino <= 100)
        {
            _Owner.thirstyDino = _Owner.thirstyDino + 0.1f;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering DrinkingState state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting DrinkingState state");
    }

    public override void UpdateState(MyAnky _Owner)
    {
        // if thirst is lower than 100
        // Drink
        // Else go back to grazing
        if (_Owner.thirstyDino <= 100)
        {
            _Owner.StartCoroutine(DrinkingWater(_Owner));
        }
        else
        {
            _Owner.myStateMachine.ChangeState(GrazingState.Instance);
        }

        // Kill Dino if health is 0
        if (_Owner.healthyDino <= 0)
        {
            _Owner.myStateMachine.ChangeState(DeadState.Instance);
        }
    }
}
