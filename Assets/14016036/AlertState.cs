using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class AlertState : State<MyAnky>
{
    private static AlertState _instance;
    Transform target;
    int[,] details;
    int changeLayer = 0;
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
        _owner.anim.SetBool("isHerding", false);
        _owner.anim.SetBool("isFleeing", false);
        _owner.anim.SetBool("isDead", false);
        int checker = 0;
        _owner.ankyFlee.enabled = false;
        //_owner.ankyFace.enabled = false;
        _owner.ankySeek.enabled = false;
        _owner.ankyWander.enabled = true;
        _owner.ankyEnemies.Clear();

        if (_owner.health > 0)
        {
            if (_owner.water < 50 && _owner.anim.GetBool("isGrazing") == true || _owner.health < 30 && _owner.anim.GetBool("isGrazing") == true)
            {
                _owner.stateMachine.ChangeState(DrinkingState.Instance);
            }
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
        details = _owner.Terrain.Terrain.terrainData.GetDetailLayer(0, 0, _owner.Terrain.Terrain.terrainData.detailWidth, _owner.Terrain.Terrain.terrainData.detailHeight, 0);
        if (details[(int)_owner.transform.position.z, (int)_owner.transform.position.x] != 0)
        {
            if (_owner.health < 60 || _owner.hunger < 50)
            {
                _owner.anky = 1;
                _owner.stateMachine.ChangeState(EatingState.Instance);
            }
        }

        for(int i = 0; i < _owner.ankyView.visibleTargets.Count; i++)
        {
            if(_owner.ankyView.visibleTargets[i].tag == "Raptor")
            {
                _owner.ankyEnemies.Add(_owner.ankyView.visibleTargets[i]);
            }
        }
        for (int i = 0; i < _owner.ankyView.stereoVisibleTargets.Count; i++)
        {
            if (_owner.ankyView.stereoVisibleTargets[i].tag == "Raptor")
            {
                _owner.ankyEnemies.Add(_owner.ankyView.stereoVisibleTargets[i]);
            }
        }
        for(int i = 0; i < _owner.ankyEnemies.Count; i++)
        {
            target = _owner.ankyView.stereoVisibleTargets[i];
            if (_owner.ankyEnemies[i].tag == "Raptor" && Vector3.Distance(target.position, _owner.transform.position) < 40 && _owner.health > 80)
            {
                _owner.attackableEnemy.Add(target);
                _owner.stateMachine.ChangeState(AttackingState.Instance);
            }
        }
    }

}





//details = _owner.ankyTerrain.terrainData.GetDetailLayer(0, 0, _owner.ankyTerrain.terrainData.detailWidth, _owner.ankyTerrain.terrainData.detailHeight, 0);
//
//details[(int)_owner.transform.position.z / 2000 * 1024, (int)_owner.transform.position.x / 2000 * 1024] = changeLayer;
//changeLayer = changeLayer + 1;
//if(changeLayer == 60)
//{
//    changeLayer = 0;
//}
//Debug.Log(changeLayer);
//_owner.ankyTerrain.terrainData.SetDetailLayer(0, 0, 0, details);
// _owner.stateMachine.ChangeState(EatingState.Instance);