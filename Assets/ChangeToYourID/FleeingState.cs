using UnityEngine;
using Statestuff;

public class FleeingState : State<MyAnky>
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

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Fleeing State");
        _owner.fleeScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Fleeing State");
        _owner.fleeScript.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {

        ////////////////////////////
        //Attack State//
        ////////////////////////////
        /*if (_owner.currentAnkyState == MyAnky.ankyState.ATTACKING)
        {
            _owner.stateMachine.ChangeState(AttackState.Instance);
        }
        ////////////////////////////
        //Alert State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
        ////////////////////////////
        //Dead State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.DEAD)
        {
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }*/
    }
}
