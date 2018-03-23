using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class SleepingState : State<MyRapty>
{

    private static SleepingState _instance; // static only declared once
                                            //private FieldOfView dinoView;

    private AStarSearch aStarScript;
    private ASPathFollower pathFollowerScript;

    int speed = 20;
    float rotationSpeed = 100.0f;

    private SleepingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static SleepingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new SleepingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("Entering Sleeping State");
        aStarScript = _owner.GetComponent<AStarSearch>();
        pathFollowerScript = _owner.GetComponent<ASPathFollower>();
        aStarScript.target = new GameObject();
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting Sleeping State");
    }


    // -- Josh --
    // The Dino will A* its way to the water sphere as it is its "home" near the water now.
    // once there, it will restore its energy level before switching back to the Idle state 
    // now that its rested.
    public override void UpdateState(MyRapty _owner)
    {
        aStarScript.target = _owner.waterDrink.gameObject;
        if (pathFollowerScript.path.nodes.Count < 1 ||
    pathFollowerScript.path == null)
        {
            pathFollowerScript.path = aStarScript.path;
        }
        move(pathFollowerScript.getDirectionVector(), _owner);
        _owner.transform.transform.localRotation = new Quaternion(0, 0, 0, 1);

        if (Vector3.Distance(_owner.waterDrink.transform.position, _owner.transform.position) < 15 && _owner.energyLevel < 100)
        {
            _owner.energyLevel += 5 * Time.deltaTime;
        }
        else if (_owner.energyLevel >= 100)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);

        }
    }

    void move(Vector3 directionVector, MyRapty _owner)
    {
        directionVector *= speed * Time.deltaTime;
        _owner.transform.Translate(directionVector, Space.World);
        _owner.transform.LookAt(_owner.transform.position + directionVector);

    }
}
