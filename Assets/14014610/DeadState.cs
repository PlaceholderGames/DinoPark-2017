using UnityEngine;
using Statestuff;

public class deadState : State<MyAnky>
{
    private static deadState _instance;

    private deadState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static deadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new deadState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering dead state");
        _owner.currentAnkyState = MyAnky.ankyState.DEAD;
        _owner.anim.SetBool("isAlerted", true);
        _owner.anim.SetBool("isAlerted", false);
        _owner.anim.SetBool("isAttacking", true);
        _owner.anim.SetBool("isAttacking", false);
        _owner.anim.SetBool("isDead", true);
        _owner.ankyWander.enabled = false;
        _owner.ankySeek.enabled = false;

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