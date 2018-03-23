using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateStuff;

public class AttackingState : State<MyRapty>
{
    private static AttackingState _Instance;
    private AttackingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static AttackingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new AttackingState();
            }

            return _Instance;
        }
    }



    public override void enterState(MyRapty _owner)
    {
        DeathScript ankyDeath = _owner.closestAnky.GetComponent<DeathScript>();
        ankyDeath.Die();
        
        Debug.Log("Entering Atacking State");

        if (_owner.health == 0)
        {
            _owner.raptyMachine.ChangeState(DeadState.Instance);
        }

        //else if (_owner.health < _owner.fleeingHealth && _owner.hunger < 50)
        //{
        //    _owner.raptyMachine.ChangeState(FleeingState.Instance);
        //}
        else
        {
            _owner.raptyMachine.ChangeState(EatingState.Instance);
        }

    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Second State");
    }

    public override void UpdateState(MyRapty _owner)
    {
       

    }
}