using UnityEngine;
using Statestuff;

public class AlertState : State<MyAnky>
{
    private static AlertState _instance;

    private AlertState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static AlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AlertState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAlert", true);
        Debug.Log("entering alert state");
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAlert", false);
        Debug.Log("exiting alertState");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(FleeingState.Instance);
        }
    }
}
