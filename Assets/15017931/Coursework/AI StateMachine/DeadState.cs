using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class DeadState : State<MyAnky>
{
    private static DeadState _instance;
    private int decayRate = 3;
    private float decayTimer = 0;

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
        Debug.Log("Entering Dead State");
        _owner.anim.SetBool("isDead", true);
        _owner.setCurrentState(MyAnky.ankyState.DEAD);
        decayTimer = decayRate;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Dead State");
        _owner.anim.SetBool("isDead", false);
    }

    public override void UpdateState(MyAnky _owner)
    {
        decayTimer -= Time.deltaTime;

        if (decayTimer <= 0)
        {
            _owner.decay(_owner.decayAmmount);
            decayTimer = decayRate;
        }

    }
}
