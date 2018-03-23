
using UnityEngine;
using Statedino;


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

    public static AttackState instance
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

    public override void Enterstate(MyAnky _onwer)
    {
        Debug.Log("Entering the first state");
    }

    public override void Exitstate(MyAnky _owner)
    {
        Debug.Log("Exiting the first state");
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.Statemachine.ChangeState(AlertState.instance);
        }
        if (_owner.switchState)
        {
            _owner.Statemachine.ChangeState(FleeingState.instance);
        }
        if (_owner.switchState)
        {
            _owner.Statemachine.ChangeState(DeadState.instance);
        }
    }
}
