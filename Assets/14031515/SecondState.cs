using UnityEngine;
using Statestuff;

public class SecondState : State<MyAnky>
{
    private static SecondState _instance;

    private SecondState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static SecondState Instance
    {
        get
        {
            if (_instance == null)
            {
                new SecondState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _o)
    {
        Debug.Log("entering Second state");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting Second state");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (!_owner.switchState)
        {
            _owner.stateMachine.ChangeState(FirstState.Instance);
        }
    }
}
