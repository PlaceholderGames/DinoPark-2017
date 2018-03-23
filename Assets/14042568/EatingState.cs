using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateStuff;

public class EatingState : State<MyRapty>
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



    public override void enterState(MyRapty _owner)
    {
       
        _owner.hunger = _owner.hunger + 50;
        if (_owner.hunger > 100)
        {
            _owner.hunger = 100;
        }
        _owner.raptyMachine.ChangeState(SearchingState.Instance);
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Second State");
    }

    public override void UpdateState(MyRapty _owner)
    {
        
    }
}