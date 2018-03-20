using UnityEngine;
using Statestuff;

public class FleeingState : State<MyAnky>
{
    private static FleeingState _instance;

    private FleeingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static FleeingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Fleeing State");
        _owner.fleeScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Fleeing State");
        _owner.fleeScript.enabled = false;
    }

    public Vector3 closestRaptor = new Vector3(10000, 10000, 10000);
    public int closestRaptorIndex = 0;
    public GameObject Raptor = new GameObject();

    public override void UpdateState(MyAnky _owner)
    {
        for (int i = 0; i < _owner.RaptorsInView.Count; i++)
        {
            Vector3 Difference = new Vector3();
            Difference = (_owner.transform.position - _owner.RaptorsInView[i].position);
            if (Difference.magnitude < closestRaptor.magnitude)
            {
                closestRaptor = Difference;
                closestRaptorIndex = 1;
                Raptor = _owner.RaptorsInView[i].gameObject;
            }
        }
        if (Raptor)
        {
            _owner.fleeScript.target = Raptor;
        }
        ////////////////////////////
        //Attack State//
        ////////////////////////////
        /*if (_owner.currentAnkyState == MyAnky.ankyState.ATTACKING)
        {
            _owner.stateMachine.ChangeState(AttackState.Instance);
        }
        ////////////////////////////
        //Alert State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
        ////////////////////////////
        //Dead State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.DEAD)
        {
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }*/
    }
}
