using UnityEngine;
using Statestuff;

public class dinkingState : State<MyAnky>
{
    private static dinkingState _instance;

    private dinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static dinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new dinkingState();
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


        if (_owner.switchState)
        {
            //_owner.stateMachine.ChangeState(SecondState.Instance);
        }
    }
}