using UnityEngine;
using Statestuff;

public class RaptyDeadState : State<MyRapty>
{
    private static RaptyDeadState _instance;

    private RaptyDeadState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptyDeadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptyDeadState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        _owner.anim.SetBool("isDead", true);
        Debug.Log("entering Dead state");
        _owner.RaptySeek.enabled = false;

    }

    public override void ExitState(MyRapty _owner)
    {
        _owner.anim.SetBool("isDead", false);
        Debug.Log("exiting DeadState");
    }

    public override void UpdateState(MyRapty _owner)
    {

    }
}
