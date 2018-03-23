using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class Alert : State<MyAnky>
{
    private static Alert _Instance;

    private Alert()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static Alert Instance
    {
        get
        {
            if (_Instance == null)
            {
                new Alert();
            }
            return _Instance;
        }
    }


    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Alert state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Alert state");
        _Owner.ankyFacing.enabled = false;
        _Owner.ankyFleeing.enabled = false;      
    }

    public override void UpdateState(MyAnky _Owner)
    {
        
        if (_Owner.raptorDistance < 25 )    //if enemy is less than 25 switch to flee
        {
            _Owner.stateMachine.ChangeState(FleeingState.Instance);
        }
        else if(_Owner.raptorDistance > 25 && _Owner.raptorDistance < 50) // if enemy near in range and over certain distance face them
        { 
            Debug.Log("flee slowly");
            _Owner.ankyFleeing.target = _Owner.RaptyEnemy;    
            _Owner.ankyFleeing.enabled = true; 
        }
        else if (_Owner.raptorDistance > 50)    // if the raptor distance is over 50 switch state back to grazing
        {
            _Owner.stateMachine.ChangeState(Grazing.Instance);
        }
        else if (_Owner.hunger <= 0 && _Owner.thirst <= 0)  // if thirst and hunger are less than 0 switch to dead state 
        {
            _Owner.stateMachine.ChangeState(DeadState.Instance);
        } 
    }
}



            //face not working 
            //Debug.Log("face");                
            //_Owner.ankyFacing.target = _Owner.Enemy;            
            //_Owner.ankyFacing.enabled = true;