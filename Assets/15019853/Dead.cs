using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class Dead : State<MyAnky>
{
    private static Dead _instance;

    private Dead()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static Dead Instance
    {
        get
        {
            if (_instance == null)
            {
                new Dead();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Death State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Leaving Death State");
    }

    public override void Updatestate(MyAnky _owner)
    {

        if (_owner.currentState == MyAnky.ankyState.DEAD)
        {
            _owner.death();
        }
    }
}
