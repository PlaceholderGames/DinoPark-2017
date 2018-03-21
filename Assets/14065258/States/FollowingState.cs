using UnityEngine;
using StateStuff;

public class FollowingState : State<MyRapty>
{
    private static FollowingState _instance; // static only declared once

    private FollowingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FollowingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FollowingState();
            }

            return _instance;
        }
    }

    public override void EnterSTate(MyRapty _owner)
    {
        Debug.Log("Entering Eating State");
        _owner.dinoWander.enabled = false;
        _owner.dinoPursue.enabled = true;
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting SecondState State");
    }

    public override void UpdateState(MyRapty _owner)
    {


        /*if (!_owner.switchState)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }*/
    }
}
