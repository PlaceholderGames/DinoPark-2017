using UnityEngine;
using Statestuff;

public class IdleState : State<MyAnky>
{
    private static IdleState _instance;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isIdle", true);
        Debug.Log("entering Idlestate");
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isIdle", false);
        Debug.Log("exiting IdleState");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(GrazeState.Instance);
        }
    }
}
