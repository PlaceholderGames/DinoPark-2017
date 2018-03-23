using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class Drinking : State<MyAnky>
{
    private static Drinking _Instance;

    private Drinking()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static Drinking Instance
    {
        get
        {
            if (_Instance == null)
            {
                new Drinking();
            }
            return _Instance;
        }
    }


    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Drinking state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Drinking state");

    }




    public override void UpdateState(MyAnky _Owner)
    {

        _Owner.thirst = 1000;



        if (_Owner.Enemy != null)   // if enemy appears go to alert
        {
            _Owner.stateMachine.ChangeState(Alert.Instance);

        }
        else if (_Owner.thirst > 900)  // else if energy is full go back to grazing
        {
            _Owner.stateMachine.ChangeState(Grazing.Instance);
        }
        else if (_Owner.hunger <= 0 && _Owner.thirst <= 0)
        {
            _Owner.stateMachine.ChangeState(DeadState.Instance);
        }


    }
}
