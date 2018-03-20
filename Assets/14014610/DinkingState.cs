using UnityEngine;
using Statestuff;

public class drinkingState : State<MyAnky>
{
    private static drinkingState _instance;

    private drinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static drinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new drinkingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering drinking state");
        _owner.anim.SetBool("isDrinking", true);

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting drinking state");
        _owner.anim.SetBool("isDrinking", false);
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.transform.position.y > 35)
        {
            _owner.stateMachine.ChangeState(grazingState.Instance);
        }
    }
}