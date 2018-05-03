using UnityEngine;
using Statestuff;

public class RaptorHuntingState : State<MyRapty>
{
    private static RaptorHuntingState _instance;

    private RaptorHuntingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptorHuntingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptorHuntingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("Entering Raptor Hunting State");
        _owner.wanderScript.enabled = true;
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Raptor Hunting State");
        _owner.wanderScript.enabled = false; 
    }

    public override void UpdateState(MyRapty _owner)
    {
        
        if(_owner.AnkyInView.Count != 0)
        {
            _owner.stateMachine.ChangeState(RaptorAlertedState.Instance);
        }
    }
}
