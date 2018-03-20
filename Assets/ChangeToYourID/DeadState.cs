using UnityEngine;
using Statestuff;

public class DeadState : State<MyAnky>
{
    private static DeadState _instance;

    private DeadState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static DeadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DeadState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Dead State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Dead State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        ////////////////////////////
        //Dead State//
        ////////////////////////////
        if (_owner.currentAnkyState == MyAnky.ankyState.DEAD)
        {
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }
    }
}
