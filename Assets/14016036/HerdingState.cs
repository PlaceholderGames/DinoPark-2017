using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class HerdingState : State<MyAnky>
{
    private static HerdingState _instance;
    GameObject closestTarget;
    private HerdingState()
    {
        if (_instance != null)
        {
            return;
        }


        _instance = this;
    }

    public static HerdingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new HerdingState();
            }
            return _instance;
        }
    }


    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Herding State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Herding State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.currentState = MyAnky.ankyState.HERDING;
        _owner.anim.SetBool("isHerding", true);
        _owner.anim.SetBool("isAlerted", false);
        _owner.anim.SetBool("isDrinking", false);
        _owner.anim.SetBool("isGrazing", false);
        _owner.anim.SetBool("isIdle", false);
        _owner.anim.SetBool("isEating", false);
        _owner.anim.SetBool("isAttacking", false);
        _owner.anim.SetBool("isFleeing", false);
        _owner.anim.SetBool("isDead", false);
        for (int i = 0; i < _owner.ankyFriendliesFar.Count; i++)
        {
            Transform target = _owner.ankyFriendliesFar[i];
            float dist = Vector3.Distance(target.position, _owner.transform.position);

            if (_owner.closestDist > dist)
            {
                _owner.closestDist = dist;
                closestTarget = target.gameObject;
            }
        }
        _owner.ankySeek.target = closestTarget;
        _owner.ankyWander.enabled = false;
        _owner.ankySeek.enabled = true;
    }
}
