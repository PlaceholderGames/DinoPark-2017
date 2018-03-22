using UnityEngine;
using Statestuff;

public class raptyDeadState : State<MyRapty>
{
    private static raptyDeadState _instance;
    private raptyDeadState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static raptyDeadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new raptyDeadState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering graze state");
        _owner.currentraptyState = MyRapty.raptyState.DEAD;
        _owner.anim.SetBool("isDead", true);
        _owner.raptySeek.enabled = false;
        _owner.raptyWander.enabled = false;
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isDead", false);

    }

    public override void UpdateState(MyRapty _owner)
    {
        _owner.health = 0;
    }
}