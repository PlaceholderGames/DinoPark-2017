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

        

        if (_Owner.raptorDistance < 25 && _Owner.raptorDistance > 5)    //enemy between 25 and 5 switch to flee
        {
            _Owner.stateMachine.ChangeState(FleeingState.Instance);
        }

        else if(_Owner.raptorDistance > 25 && _Owner.raptorDistance < 50) // if enemy near in range and over certain distance face them
        {
            //face not working 
            //Debug.Log("face");                
            //_Owner.ankyFacing.target = _Owner.Enemy;            
            //_Owner.ankyFacing.enabled = true;
            
            Debug.Log("flee slowly");
            _Owner.ankyFleeing.target = _Owner.Enemy;    
            _Owner.ankyFleeing.enabled = true;            
           
        }

        else if (_Owner.raptorDistance > 50)
        {

            _Owner.stateMachine.ChangeState(Grazing.Instance);
        }
        else if (_Owner.hunger <= 0 && _Owner.thirst <= 0)
        {
            _Owner.stateMachine.ChangeState(DeadState.Instance);
        }


        //start facing threat then move away if too close

        // can go to attack, flee, eat drink or grazing.


    }
}
