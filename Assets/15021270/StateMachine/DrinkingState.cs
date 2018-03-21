using UnityEngine;
using StateStuff;

public class DrinkingState : State<AI>
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

    public override void EnterState(AI _owner)
    {

        Debug.Log("Entering Drinking State");
    }

    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting Drinking State");
    }

    public override void UpdateState(AI _owner)
    {

        //_owner.stateMachine.ChangeState(HuntingState.Instance);
    }
}