using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using UnityEditor;

public class BreedingState : State<MyRapty>
{

    private static BreedingState _instance; // static only declared once
                                            //private FieldOfView dinoView;
    private AStarSearch aStarScript;
    private ASPathFollower pathFollowerScript;
    private Vector3 pos;
    public bool breed = true;
    int speed = 20;

    private BreedingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static BreedingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new BreedingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        aStarScript = _owner.GetComponent<AStarSearch>();
        pathFollowerScript = _owner.GetComponent<ASPathFollower>();

        Debug.Log("Entering Breeding State");
    }

    public override void ExitState(MyRapty _owner)
    {
        _owner.breeding = false;
        Debug.Log("exiting Breeding State");
        //_owner.dinoWander.enabled = false;
        //_owner.dinoView.enabled = false;
    }

    public override void UpdateState(MyRapty _owner)
    {
        for (int i = 0; i < _owner.dinoView.visibleTargets.Count; i++)
        {
            if (_owner.dinoView.visibleTargets[i] != null)
            {
                if (_owner.dinoView.visibleTargets[i].CompareTag("Rapty"))
                {
                    if (Vector3.Distance(_owner.dinoView.visibleTargets[i].transform.position, _owner.transform.position) < 20 && breed)
                    {
                        Debug.Log("Breeding");
                        //((Box.transform.position - Capsule.transform.position) * 0.5f) + Capsule.transform.position;
                        _owner.breed(((_owner.dinoView.visibleTargets[i].transform.position - _owner.transform.position) * 0.5f) + _owner.transform.position);
                        breed = false;
                        _owner.stateMachine.ChangeState(IdleState.Instance);
                        //MyRapty.Instantiate<GameObject>(_owner.gameObject);
                    }
                    else
                    {
                        aStarScript.target = _owner.dinoView.visibleTargets[i].gameObject;

                        if (pathFollowerScript.path.nodes.Count < 1 || pathFollowerScript.path == null)
                        {
                            pathFollowerScript.path = aStarScript.path;
                        }
                        move(pathFollowerScript.getDirectionVector(), _owner);
                        _owner.transform.transform.localRotation = new Quaternion(0, 0, 0, 1);
                    }
                }

            }
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
