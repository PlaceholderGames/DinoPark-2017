﻿using UnityEngine;
using Statestuff;

public class DeadState : State<MyAnky>
{
    private static DeadState _instance;

    private DeadState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static DeadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DeadState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isDead", true);
        Debug.Log("entering Dead state");
        _owner.ankyFlee.enabled = false;
        _owner.ankySeek.enabled = false;
        _owner.ankyWander.enabled = false;
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isDead", false);
        Debug.Log("exiting DeadState");
    }

    public override void UpdateState(MyAnky _owner)
    {
       //you are dead now.
    }
}
