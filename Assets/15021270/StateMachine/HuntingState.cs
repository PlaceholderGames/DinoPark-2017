using UnityEngine;
using StateStuff;

public class HuntingState : State<AI>
{
    private static HuntingState _instance;

    private HuntingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static HuntingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new HuntingState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        _owner.wander.enabled = false;
        //_owner.face.enabled = true;
        _owner.pursue.enabled = true;

        Debug.Log("Entering Hunting State");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Hunting State");
    }

    public override void UpdateState(AI _owner)
    {
        _owner.hunt();
        //Update while in hunting state
        //_owner.SetSteering(_owner.face.GetSteering());
        //_owner.SetSteering(_owner.pursue.GetSteering());

        //_owner.stateMachine.ChangeState(HuntingState.Instance);
    }
}