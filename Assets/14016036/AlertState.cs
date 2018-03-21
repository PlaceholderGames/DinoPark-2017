using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class AlertState : State<MyAnky>
{
    private static AlertState _instance;
    Transform target;
    private AlertState()
    {
        if (_instance != null)
        {
            return;
        }


        _instance = this;
    }

    public static AlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AlertState();
            }
            return _instance;
        }
    }


    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Alert State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Alert State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.ankyFriendliesClose.Clear();
        _owner.ankyFriendliesFar.Clear();
        _owner.currentState = MyAnky.ankyState.ALERTED;
        _owner.anim.SetBool("isAlerted", true);
        _owner.anim.SetBool("isDrinking", false);
        _owner.anim.SetBool("isGrazing", false);
        _owner.anim.SetBool("isIdle", false);
        _owner.anim.SetBool("isEating", false);
        _owner.anim.SetBool("isAttacking", false);
        _owner.anim.SetBool("isFleeing", false);
        _owner.anim.SetBool("isDead", false);
        int checker = 0;
        _owner.ankyFlee.enabled = false;
        _owner.ankyFace.enabled = false;
        _owner.ankySeek.enabled = false;
        _owner.ankyWander.enabled = true;
        _owner.ankyEnemies.Clear();

        if (_owner.transform.position.y < 35.5 && _owner.anim.GetBool("isAlerted") == true)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
            //_owner.currentState = MyAnky.ankyState.DRINKING;
            
        }
        for (int i = 0; i < _owner.ankyView.visibleTargets.Count; i++)
        {
            Transform target = _owner.ankyView.visibleTargets[i];
            if (_owner.ankyView.visibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, _owner.transform.position) > 150)
            {
                _owner.anky = 1;
                _owner.stateMachine.ChangeState(WanderingState.Instance);
                //Debug.Log("its getting in this Alert");

            }
        }
        for (int i = 0; i < _owner.ankyView.stereoVisibleTargets.Count; i++)
        {
            Transform target = _owner.ankyView.stereoVisibleTargets[i];
            if (_owner.ankyView.stereoVisibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, _owner.transform.position) > 150)
            {
                _owner.anky = 1;
                _owner.stateMachine.ChangeState(WanderingState.Instance);
                //Debug.Log("its getting in this Alert");
        
            }
        }
        _owner.anky = 0;
        for (int i = 0; i < _owner.ankyView.visibleTargets.Count; i++)
        {
            target = _owner.ankyView.visibleTargets[i];

            if (_owner.ankyView.visibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, _owner.transform.position) < 40)
            {
                _owner.ankyEnemies.Add(target);
                _owner.ankyFlee.target = _owner.ankyView.visibleTargets[i].gameObject;
                _owner.fleeingIndex = i;
                //_owner.ankyAgent.maxSpeed = 3;
                _owner.stateMachine.ChangeState(FleeingState.Instance);
                checker++;
                Debug.Log(checker);
            }
        }
        for (int i = 0; i < _owner.ankyView.stereoVisibleTargets.Count; i++)
        {
            Transform target = _owner.ankyView.stereoVisibleTargets[i];
        
            if (_owner.ankyView.stereoVisibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, _owner.transform.position) < 40)
            {
                _owner.ankyEnemies.Add(target);
                _owner.ankyFlee.target = _owner.ankyView.stereoVisibleTargets[i].gameObject;
                _owner.fleeingIndex = i;
                //_owner.ankyAgent.maxSpeed = 3;
                _owner.stateMachine.ChangeState(FleeingState.Instance);
                checker++;
                Debug.Log(checker);
            }
        }


        _owner.findFriendlies();


        for (int i = 0; i < _owner.ankyView.visibleTargets.Count; i++)
        {
            target = _owner.ankyView.visibleTargets[i];
            if (_owner.ankyView.visibleTargets[i].tag == "Anky")
            {
                if (Vector3.Distance(target.position, _owner.transform.position) > 60)
                {
                    _owner.stateMachine.ChangeState(HerdingState.Instance);
                }
            }
        }
        for (int i = 0; i < _owner.ankyView.stereoVisibleTargets.Count; i++)
        {
            target = _owner.ankyView.stereoVisibleTargets[i];
            if (_owner.ankyView.stereoVisibleTargets[i].tag == "Anky")
            {
                if (Vector3.Distance(target.position, _owner.transform.position) > 60)
                {
                    _owner.stateMachine.ChangeState(HerdingState.Instance);
                }
            }
        }

        //if(_owner.ankyTerrain.terrainData.GetDetailLayer(0, 0, 0, 0, 1) && _owner.health < 70)
    }

}
