using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class Breed : State<MyAnky>
{
    private static Breed _instance;

    private Breed()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static Breed Instance
    {
        get
        {
            if (_instance == null)
            {
                new Breed();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Breed State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Leaving Breed State");
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.currentState == MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(Grazing.Instance);
        }
        else
        {
            //_owner.breed();
        }
    }
}
