using UnityEngine;
using StateStuff;
using System.Collections.Generic;
using System.Collections;

public class DrinkingState : State<MyRapty>
{
    private static DrinkingState _instance; // static only declared once
    int speed = 20;
    float rotationSpeed = 100.0f;

    private AStarSearch aStarScript;
    private ASPathFollower pathFollowerScript;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static DrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }

            return _instance;
        }
    }
    public override void EnterState(MyRapty _owner)
    {
        aStarScript = _owner.GetComponent<AStarSearch>();
        pathFollowerScript = _owner.GetComponent<ASPathFollower>();

        //_owner.dinoPursue.enabled = true;
        Debug.Log("Entering Drinking State");
    }

    public override void ExitState(MyRapty _owner)
    {

        //_owner.dinoPursue.target = new GameObject();

        Debug.Log("exiting Drinking State");
    }

    public override void UpdateState(MyRapty _owner)
    {

        _owner.waterDrink.transform.position = aStarScript.mapGrid.getEdgeWater(_owner.gameObject).position;
        aStarScript.target = new GameObject();
        aStarScript.target = _owner.waterDrink.gameObject;
        if (pathFollowerScript.path.nodes.Count < 1 || 
            pathFollowerScript.path == null)
        {
            pathFollowerScript.path = aStarScript.path;
        }
        move(pathFollowerScript.getDirectionVector(), _owner);
        //_owner.transform.transform.localRotation = new Quaternion(0, 0, 0, 1);

        if (Vector3.Distance(_owner.waterDrink.transform.position, _owner.transform.position) < 25 && _owner.waterLevel < 100)
        {
            _owner.waterLevel += 20 * Time.deltaTime;
        }
        else if(_owner.waterLevel >= 100)
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
