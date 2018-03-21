using UnityEngine;
using StateStuff;

public class EatingState : State<AI>
{
    private static EatingState _instance;

    private EatingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatingState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {

        Debug.Log("Entering Eating State");
    }

    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting Eating State");
    }

    public override void UpdateState(AI _owner)
    {

        //_owner.stateMachine.ChangeState(HuntingState.Instance);
    }
}