using UnityEngine;
using Statestuff;

public class AttackState : State<MyAnky>
{
    private static AttackState _instance;

    private AttackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static AttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Attack State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Attack State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        ////////////////////////////
        //Fleeing State//
        ////////////////////////////
        if (_owner.currentAnkyState == MyAnky.ankyState.FLEEING)
        {
            _owner.stateMachine.ChangeState(FleeingState.Instance);
        }
        ////////////////////////////
        //Alert State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.ALERTED)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
        ////////////////////////////
        //Dead State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.DEAD)
        {
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }
    }
}
