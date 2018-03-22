using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class AttackingState : State<MyAnky>
{
    private static AttackingState _instance;
    
    private AttackingState()
    {
        if (_instance != null)
        {
            return;
        }


        _instance = this;
    }

    public static AttackingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackingState();
            }
            return _instance;
        }
    }


    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Attacking State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(MyAnky _owner)
    {

        _owner.ankySeek.target = _owner.attackableEnemy[0].gameObject;
        _owner.ankySeek.enabled = true;
        if (Vector3.Distance(_owner.ankySeek.target.transform.position, _owner.transform.position) < 4)
        {
            _owner.ankySeek.enabled = false;
            _owner.time -= Time.deltaTime;
        }
        if(_owner.time == 0)
        {
            _owner.animClip.Play("Ank_Attack");
            Debug.Log("I've done dmg");
        }
        _owner.time = 5;
    }
    
}
