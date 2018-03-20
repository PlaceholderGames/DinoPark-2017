using UnityEngine;
using Statestuff;

public class EatingingState : State<MyAnky>
{
    private static EatingingState _instance;

    private EatingingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static EatingingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatingingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isEating", true);
        Debug.Log("entering Eating state");
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isGrazing", false);
        Debug.Log("exiting EatingState");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
    }
}
