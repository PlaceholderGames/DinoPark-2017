using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class FleeingState : State<MyAnky>
{
    private static FleeingState _Instance;

    public float weight = 1.0f;

    private FleeingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static FleeingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new FleeingState();
            }
            return _Instance;
        }
    }


    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Fleeing state");

        
      
        //_Owner.SetSteering(_Owner.fleeing.GetSteering(), _Owner.weight);
        
        

    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Fleeing state");
        _Owner.maxSpeed = 1.0f;
        _Owner.ankyFleeing.enabled = false;
    }

    public override void UpdateState(MyAnky _Owner)
    {
        
        if (_Owner.raptorDistance > 25) // if enemy distance is over 25 switch back to alert state
        {
            _Owner.ankyFleeing.enabled = false;
            _Owner.stateMachine.ChangeState(Alert.Instance);    
        }
        else if (_Owner.raptorDistance < 25 && _Owner.raptorDistance > 5)   // if raptor distance is less than 25 but over 5 flee from the raptor
        {
            _Owner.ankyFleeing.target = _Owner.RaptyEnemy;   // set flee target to raptor
            _Owner.ankyFleeing.enabled = true;              // start fleeing from raptor
        }
        else if (_Owner.raptorDistance < 5) // if raptor distance is less than 5 switch to attack state
        {
            _Owner.stateMachine.ChangeState(AttackState.Instance);
        }
        else if (_Owner.hunger <= 0 && _Owner.thirst <= 0)  // if hunger and thirst are below 0 then switch to dead state
        {
            _Owner.stateMachine.ChangeState(DeadState.Instance);
        }

    }
}


//_Owner.maxSpeed = 20.0f;    // doesnt increase speed