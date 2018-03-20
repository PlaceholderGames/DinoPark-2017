using UnityEngine;
using StateStuff;

public class IdleState : State<AI>
{
    private static IdleState _instance;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        _owner.face.enabled = true;
        _owner.pursue.enabled = false;

        Debug.Log("Entering idle State");
    }

    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting idle State");
    }

    public override void UpdateState(AI _owner)
    {
        foreach (Transform i in _owner.view.visibleTargets)
        {
            if (i.tag == "Anky")
            {
                _owner.stateMachine.ChangeState(HuntingState.Instance);
            }
        }
        
    }
}