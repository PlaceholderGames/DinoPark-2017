using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
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

    IEnumerator Deaddo (MyAnky _Owner)
    {
        yield return new WaitForSeconds(1);
        _Owner.myTransform.Rotate(0, 0, 180);
    }

    public override void EnterState(MyAnky _Owner)
    {
        // Disables all scripts
        // Flips the dino
        Debug.Log("entering Dead state");

        _Owner.myAnkyAgent.enabled = false;
        _Owner.myAnkyFlee.enabled = false;
        _Owner.myAnkyWander.enabled = false;
        _Owner.myAnkyHerd.enabled = false;
        _Owner.myAnkyAStar.enabled = false;
        _Owner.myAnkyAStarInstance.enabled = false;
        _Owner.myAnkyPathFollower.enabled = false;
        _Owner.myAnim.enabled = false;
        _Owner.myView.enabled = false;
        _Owner.StartCoroutine(Deaddo(_Owner));
        _Owner.myAnky.enabled = false;
        
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Dead state");
    }

    public override void UpdateState(MyAnky _Owner)
    {
    }
}
