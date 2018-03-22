using UnityEngine;
using Statestuff;

public class ankyDeadState : State<MyAnky>
{
    private static ankyDeadState _instance;

    private ankyDeadState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static ankyDeadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ankyDeadState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering dead state");
        _owner.currentAnkyState = MyAnky.ankyState.DEAD;
        _owner.anim.SetBool("isDead", true);
        _owner.ankyWander.enabled = false;
        _owner.ankySeek.enabled = false;
        _owner.ankyFlee.enabled = false;

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting dead state");
        _owner.anim.SetBool("isDead", false);
    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.health = 0;
    }
}