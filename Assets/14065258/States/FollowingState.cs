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
       
        Debug.Log("Entering Following State");
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting Following State");
    }

    // -- Josh --
    // Alot happens here so i'll comment it line by line.
    public override void UpdateState(MyRapty _owner)
    {
        // If no dino is in visible range, dont do anything.
        if (_owner.dinoView.visibleTargets.Count != 0)
        {
            // Get the dino we identfied as the one we want to kill
            targetDinoLocation = _owner.dinoView.visibleTargets[_owner.targetDinoLocation].position;
            
            // if we are in range to do damage..
            if (Vector3.Distance(targetDinoLocation, _owner.transform.position) < 20)
            {
                // start dealing damamge to that dino.
                isdead = _owner.dinoView.visibleTargets[_owner.targetDinoLocation].gameObject.GetComponent<MyAnky>().takeDamage(10);

                // When we finially get the message back that it is dead
                // we increase our hunger and go back to the idle state.
                if (isdead)
                {
                    _owner.hungerLevel += 70;

                    _owner.stateMachine.ChangeState(IdleState.Instance);
                }
            }
            // however, if we are not in view of the dino, we A* towards it.
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
        // If we dont see a dino in our side view, we check our front view and A* towards the first thing we see
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
        // if our energy runs out from all the hard work, we go to the sleeping state
        if (_owner.energyLevel <= 0)
        {
            _owner.stateMachine.ChangeState(SleepingState.Instance);
        }
        // or if our water runs out, we go get a drink because you cant fight
        // with a dry mouth. 
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
