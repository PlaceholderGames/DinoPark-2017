﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class HerdingState : State<MyAnky>
{
    private static HerdingState _Instance;

    public float weight = 1.0f;

    private HerdingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static HerdingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new HerdingState();
            }
            return _Instance;
        }
    }


    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Herding state");
       
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Herding state");
        _Owner.ankyHerding.enabled = false;
    }

    public override void UpdateState(MyAnky _Owner)
    {
        _Owner.ankyHerding.target = _Owner.AnkyFriend;  // set target      

        if (_Owner.ankyDistance > 50)   // if the anky is over 50 away start herding 
        {
            _Owner.ankyHerding.enabled = true;
        }
        else if (_Owner.ankyDistance < 20)  // if the anky distance is under 20 switch back to grazing
        {
            //Debug.Log(_Owner.ankyDistance);
            _Owner.stateMachine.ChangeState(Grazing.Instance);
            
        }
        else if (_Owner.RaptyEnemy != null && _Owner.raptorDistance < 50) // if herding and a raptor appears become alerted
        {
            Debug.Log("whilst herding a raptor appeared");
            _Owner.stateMachine.ChangeState(Alert.Instance);
        }
        else if (_Owner.hunger <= 0 && _Owner.thirst <= 0)  // if thirst and hunger below 0 switch to dead
        {
            _Owner.stateMachine.ChangeState(DeadState.Instance);    
        }      

    }
}
