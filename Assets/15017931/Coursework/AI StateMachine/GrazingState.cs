using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class GrazingState : State<MyAnky>
{
    private static GrazingState _instance;

    private GrazingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static GrazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Grazing State");
        _owner.anim.SetBool("isGrazing", true);
        _owner.setCurrentState(MyAnky.ankyState.GRAZING);
        _owner.wanderBehaviourScript.enabled = true;

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Grazing State");
        _owner.anim.SetBool("isGrazing", false);
        _owner.wanderBehaviourScript.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        //Alert / arent dead
        if (_owner.predatorsInRange.Count > 0 || _owner.myStats.health <= 0)
        {
            _owner.wanderBehaviourScript.enabled = true;
            Debug.Log(_owner.predatorsInRange.Count);
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
        //Eating
        if (_owner.myStats.hunger < 50)
            //If we arent on grass, go to grass
            if (_owner.gameObject.transform.position.y < 52 || _owner.gameObject.transform.position.y > 94)
            {

                //Use A* to find closest point on map that is at y 52 - 94 (grass)
                _owner.aStarScript.target = _owner.gameObject;
                _owner.aStarTarget.transform.position = _owner.aStarScript.mapGrid.getFoodSource(_owner.gameObject).position;
                _owner.aStarScript.target = _owner.aStarTarget.gameObject;

                if (_owner.pathFollowerScript.path.nodes.Count < 1 || _owner.pathFollowerScript.path == null)
                {
                    _owner.pathFollowerScript.path = _owner.aStarScript.path;
                }
                _owner.move(_owner.pathFollowerScript.getDirectionVector());

            }
            else
            {
                _owner.anim.SetBool("isEating", true);
                _owner.stateMachine.ChangeState(EatingState.Instance);
            }

        //Drinking
        else if (_owner.myStats.thirst < 50)
        {
            Debug.Log("Searching for Water");
            //If we arent on water, go to water
            if (_owner.gameObject.transform.position.y > 36)
            {
                //Use A* to find closest point on map that is at y 36 or less (water level)
                _owner.aStarScript.target = _owner.gameObject;
                _owner.aStarTarget.transform.position = _owner.aStarScript.mapGrid.getWaterEdge(_owner.gameObject).position;
                _owner.aStarScript.target = _owner.aStarTarget.gameObject;

                if (_owner.pathFollowerScript.path.nodes.Count < 1 || _owner.pathFollowerScript.path == null)
                {
                    _owner.pathFollowerScript.path = _owner.aStarScript.path;
                }
                _owner.move(_owner.pathFollowerScript.getDirectionVector());
            }
            else
            {
                _owner.anim.SetBool("isDrinking", true);
                _owner.stateMachine.ChangeState(DrinkingState.Instance);
            }
        }
        else
        {
            _owner.herd();
        }
            //Idle
        //_owner.stateMachine.ChangeState(IdleState.Instance);
    }
}
