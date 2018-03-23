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

        Debug.Log("Entering Drinking State");
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting Drinking State");
    }

    // -- Josh --
    // This will call the GetEdgeWAter from the MapGrid script and will move our watersphere to that location
    // it will then A* its way over to that location
    // Once in range, the water level will increase before
    // when it's water is back to full, it will go back into idle

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
