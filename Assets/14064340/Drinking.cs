using UnityEngine;
using Statestuff;

public class DrinkingState : State<MyAnky>
{
    private static DrinkingState _instance;

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

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isDrinking", true);
        Debug.Log("entering DrinkingState");
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isDrinking", false);
        Debug.Log("exiting DrinkingState");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
    }
}
