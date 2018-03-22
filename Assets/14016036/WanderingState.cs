using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class WanderingState : State<MyAnky>
{
    private static WanderingState _instance;
    Transform target;
    int[,] details;
    private WanderingState()
    {
        if (_instance != null)
        {
            return;
        }


        _instance = this;
    }

    public static WanderingState Instance
    {
        get
        {
            if(_instance == null)
            {
                new WanderingState();
            }
            return _instance;
        }
    }


    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Wandering State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Wandering State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.currentState = MyAnky.ankyState.GRAZING;
        //_owner.ankyFace.enabled = false;
        for (int i = 0; i < _owner.ankyView.visibleTargets.Count; i++)
        {
            Transform target = _owner.ankyView.visibleTargets[i];
            if (_owner.ankyView.visibleTargets[i].tag == "Rapty" && Vector3.Distance(target.position, _owner.transform.position) <= 150)
            {
                _owner.anky = 1;
                _owner.anim.SetTrigger("isAlert");
                _owner.anim.SetBool("isAlerted", true);
                _owner.anim.SetBool("isDrinking", false);
                _owner.anim.SetBool("isGrazing", false);
                _owner.anim.SetBool("isIdle", false);
                _owner.anim.SetBool("isEating", false);
                _owner.anim.SetBool("isAttacking", false);
                _owner.anim.SetBool("isFleeing", false);
                _owner.anim.SetBool("isDead", false);

                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
        }
        if (_owner.anky <= 0)
        {
            _owner.anim.SetBool("isAlerted", false);
            _owner.anim.SetBool("isDrinking", false);
            _owner.anim.SetBool("isGrazing", true);
            _owner.anim.SetBool("isIdle", false);
            _owner.anim.SetBool("isEating", false);
            _owner.anim.SetBool("isAttacking", false);
            _owner.anim.SetBool("isFleeing", false);
            _owner.anim.SetBool("isDead", false);
        }
        _owner.anky = 0;
        if (_owner.health > 0)
        {
            if (_owner.water < 50 && _owner.anim.GetBool("isGrazing") == true || _owner.health < 30 && _owner.anim.GetBool("isGrazing") == true )
            {
                _owner.anky = 1;
                _owner.stateMachine.ChangeState(DrinkingState.Instance);
            }
        }
        _owner.anky = 0;




        _owner.findFriendlies();

        for (int i = 0; i < _owner.ankyView.visibleTargets.Count; i++)
        {
            target = _owner.ankyView.visibleTargets[i];
            if (_owner.ankyView.visibleTargets[i].tag == "Anky")
            {
                if (Vector3.Distance(target.position, _owner.transform.position) > 60)
                {
                    _owner.anky = 1;
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
                    _owner.anky = 1;
                    _owner.stateMachine.ChangeState(HerdingState.Instance);
                }
            }
        }
        details = _owner.Terrain.Terrain.terrainData.GetDetailLayer(0, 0, _owner.Terrain.Terrain.terrainData.detailWidth, _owner.Terrain.Terrain.terrainData.detailHeight, 0);
        if (details[(int)_owner.transform.position.z, (int)_owner.transform.position.x] != 0)
        {
            if (_owner.health < 60)
            {
                _owner.anky = 1;
                _owner.stateMachine.ChangeState(EatingState.Instance);
            }
        }
        _owner.anky = 0;

        //details = _owner.ankyTerrain.terrainData.GetDetailLayer(0, 0, _owner.ankyTerrain.terrainData.detailWidth, _owner.ankyTerrain.terrainData.detailHeight, 0);
        //
        //details[(int)_owner.transform.position.z / 2000 * 1024, (int)_owner.transform.position.x / 2000 * 1024] = 2;
        //
        //_owner.ankyTerrain.terrainData.SetDetailLayer(0, 0, 0, details);
    }
}
