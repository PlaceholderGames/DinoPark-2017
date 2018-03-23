using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateStuff;

public class IdleState : State<MyRapty>
{
    private static IdleState _instance; // static only declared once

    public bool breed = true;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("Entering Idle State");
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting Idle State");
    }

    // -- Josh -- 
    // The Dino will call the movement code from MyRapty and will just idle around the map until one of its
    // levels gets below a certain before it will then switch states to that state.
    public override void UpdateState(MyRapty _owner)
    {
        if (_owner.energyLevel <= 0)
        {
            _owner.stateMachine.ChangeState(SleepingState.Instance);
        }
        if (_owner.waterLevel < 20)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
        }
        if (_owner.hungerLevel < 30)
        {
            _owner.stateMachine.ChangeState(HuntingState.Instance);
        }

        // ----------
        // -- Josh --
        // Uncomment to test the breeding code.
        /*
        for (int i = 0; i < raptorArray.Length; i++)
        {
            if (raptorArray.Length <= 2 && !raptorArray[i].GetComponent<MyRapty>().breeding)
            {
                _owner.stateMachine.ChangeState(BreedingState.Instance);
                _owner.breeding = true;
            }
        } */
        // ----------


        _owner.movement(_owner, _owner.moveSpeed);
    }

   
}
