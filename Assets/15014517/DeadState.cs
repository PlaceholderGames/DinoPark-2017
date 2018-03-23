using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class DeadState : State<MyAnky>
{
    private static DeadState _Instance;

    private DeadState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static DeadState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new DeadState();
            }
            return _Instance;
        }
    }


    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("entering Dead state");
        _Owner.ankyFacing.enabled = false;
        _Owner.ankyHerding.enabled = false;
        _Owner.ankyFleeing.enabled = false;
        _Owner.ankyWandering.enabled = false;
        
        _Owner.StartCoroutine(DIE(_Owner));
        _Owner.ankyView.enabled = false;
        //_Owner.agent.enabled = false;
        _Owner.myAnky.enabled = false;
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Dead state");

    }

    IEnumerator DIE (MyAnky _Owner)
    {
        yield return new WaitForSeconds(1);
        _Owner.myTransform.Rotate(0, 0, 180);
    }

    public override void UpdateState(MyAnky _Owner)
    {
        Debug.Log("Anky is Dead");
        
    }
}
