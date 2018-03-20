using UnityEngine;
using Statestuff;

public class GrazeState : State<MyAnky>
{
    private static GrazeState _instance;

    private GrazeState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static GrazeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazeState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isGrazing", true);
        Debug.Log("entering Grazingstate");
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isGrazing", false);
        Debug.Log("exiting GrazeState");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
    }
}
