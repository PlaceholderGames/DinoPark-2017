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
        _owner.anim.SetBool("isFleeing", true);
        Debug.Log("entering Fleeing state");
        _owner.ankyFlee.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isFleeing", false);
        Debug.Log("exiting FleeingState");
        _owner.ankyFlee.enabled = false;
    }

    public GameObject priorityTarget = new GameObject();
    public override void UpdateState(MyAnky _owner)
    {
        foreach(Transform x in _owner.Enemies)
        {
            Vector3 Difference = new Vector3();
            Vector3 RaptorDiff = new Vector3();

            Difference = (_owner.transform.position - x.position);
            RaptorDiff = (_owner.transform.position - priorityTarget.transform.position);

            if (Difference.magnitude < RaptorDiff.magnitude)
            {
                priorityTarget = x.gameObject;
            }
            float Distance = Vector3.Distance(x.position, _owner.transform.position);
            if (Distance > 50)
            {
                _owner.Enemies.Clear();
                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
        }

        if(priorityTarget)
        {
            _owner.ankyFlee.target = priorityTarget;
        }
     
    }
}
