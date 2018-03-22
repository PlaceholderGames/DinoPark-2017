using UnityEngine;
using StateStuff;
using System.Collections.Generic;
using System.Collections;

public class FollowingState : State<MyRapty>
{
    private static FollowingState _instance; // static only declared once
    int speed = 20;
    float rotationSpeed = 100.0f;
    bool isdead = false;

    Vector3 targetDinoLocation;

    private AStarSearch aStarScript;
    private ASPathFollower pathFollowerScript;

    private FollowingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FollowingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FollowingState();
            }

            return _instance;
        }
    }
    public override void EnterState(MyRapty _owner)
    {
        aStarScript = _owner.GetComponent<AStarSearch>();
        pathFollowerScript = _owner.GetComponent<ASPathFollower>();
       
        //_owner.dinoPursue.enabled = true;
        Debug.Log("Entering Following State");
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting Following State");
    }

    public override void UpdateState(MyRapty _owner)
    {

        //Debug.Log(Vector3.Distance(_owner.dinoView.visibleTargets[_owner.targetDinoLocation].position, _owner.transform.position));
        if (_owner.dinoView.visibleTargets.Count != 0)
        {
            targetDinoLocation = _owner.dinoView.visibleTargets[_owner.targetDinoLocation].position;

            if (Vector3.Distance(targetDinoLocation, _owner.transform.position) < 20)
            {
                isdead = _owner.dinoView.visibleTargets[_owner.targetDinoLocation].gameObject.GetComponent<MyAnky>().takeDamage(10);

                if (isdead)
                {
                    _owner.hungerLevel += 70;

                    _owner.stateMachine.ChangeState(IdleState.Instance);
                }
            }
            else
            {
                aStarScript.target = _owner.dinoView.visibleTargets[_owner.targetDinoLocation].gameObject;

                if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
                {
                    pathFollowerScript.path = aStarScript.path;
                }
                move(pathFollowerScript.getDirectionVector(), _owner);
                _owner.transform.transform.localRotation = new Quaternion(0, 0, 0, 1);
            }
        }
        else if (_owner.dinoView.stereoVisibleTargets.Count != 0)
        {
            aStarScript.target = _owner.dinoView.stereoVisibleTargets[0].gameObject;

            if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
            {
                pathFollowerScript.path = aStarScript.path;
            }
            move(pathFollowerScript.getDirectionVector(), _owner);
            _owner.transform.transform.localRotation = new Quaternion(0, 0, 0, 1);
        }
        if (_owner.energyLevel <= 0)
        {
            _owner.stateMachine.ChangeState(SleepingState.Instance);
        }
        if (_owner.waterLevel < 20)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
        }
    }


    void move(Vector3 directionVector, MyRapty _owner)
    {
        directionVector *= speed * Time.deltaTime;
        if (_owner.dinoView.visibleTargets.Count == 0 && _owner.dinoView.stereoVisibleTargets.Count == 0)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }
        _owner.transform.Translate(directionVector, Space.World);
        _owner.transform.LookAt(_owner.transform.position + directionVector);

    }
}
