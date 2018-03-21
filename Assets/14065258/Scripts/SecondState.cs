using UnityEngine;
using StateStuff;

public class SecondState : State<AI>
{
    private static SecondState _instance; // static only declared once

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

    public override void EnterSTate(AI _owner)
    {
        Debug.Log("Entering Second State");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("exiting SecondState State");
    }

    public override void UpdateState(AI _owner)
    {
        if (!_owner.switchState)
        {
            _owner.stateMachine.ChangeState(FirstState.Instance);
        }
    }
}
