using UnityEngine;
using StateStuff;

public class FleeingState : State<AI>
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

    public override void EnterState(AI _owner)
    {

        Debug.Log("Entering Fleeing State");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Fleeing State");
    }

    public override void UpdateState(AI _owner)
    {


        //_owner.stateMachine.ChangeState(DeadState.Instance);

        //_owner.stateMachine.ChangeState(HuntingState.Instance);
    }
}