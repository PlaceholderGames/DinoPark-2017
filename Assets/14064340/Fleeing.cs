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
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isFleeing", false);
        Debug.Log("exiting FleeingState");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
    }
}
