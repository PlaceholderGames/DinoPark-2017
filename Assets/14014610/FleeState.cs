using UnityEngine;
using Statestuff;

public class fleeState : State<MyAnky>
{
    private static fleeState _instance;

    private fleeState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static fleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new fleeState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering flee state");
        _owner.anim.SetBool("isFleeing", true);
        _owner.ankyFlee.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting flee state");
        _owner.anim.SetBool("isFleeing", false);
        _owner.ankyFlee.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {


        if (_owner.switchState)
        {
            //_owner.stateMachine.ChangeState(SecondState.Instance);
        }
    }
}