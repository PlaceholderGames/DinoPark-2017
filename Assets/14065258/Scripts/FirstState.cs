using UnityEngine;
using StateStuff;

public class FirstState : State<AI>
{
    private static FirstState _instance; // static only declared once

    private FirstState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FirstState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FirstState();
            }

            return _instance;
        }
    }

    public override void EnterSTate(AI _owner)
    {
        Debug.Log("Entering First STate");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("exiting First STate");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(SecondState.Instance);
        }
    }
}
