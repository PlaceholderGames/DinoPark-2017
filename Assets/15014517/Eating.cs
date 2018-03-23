using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class Eating : State<MyAnky>
{
    private static Eating _Instance;

    private Eating()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static Eating Instance
    {
        get
        {
            if (_Instance == null)
            {
                new Eating();
            }
            return _Instance;
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
        _Owner.hunger = _Owner.hunger + 1;
        if (_Owner.RaptyEnemy != null)   // if enemy appears go to alert
        {
            _Owner.stateMachine.ChangeState(Alert.Instance);            
        }
        else if (_Owner.hunger > 1000)  // else if hunger is over 900 back to grazing
        {
            _Owner.stateMachine.ChangeState(Grazing.Instance);
        }
        else if (_Owner.hunger <= 0 && _Owner.thirst <= 0)  // if  hunger and thirst are 0 then switch to dead state
        {
            _Owner.stateMachine.ChangeState(DeadState.Instance);
        }
    }
}
