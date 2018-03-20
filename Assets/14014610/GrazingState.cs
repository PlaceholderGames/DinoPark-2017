using UnityEngine;
using Statestuff;

public class grazingState : State<MyAnky>
{
    private static grazingState _instance;

    private grazingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static grazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new grazingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering graze state");
        _owner.anim.SetBool("isGrazing", true);
        _owner.ankyWander.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isGrazing", false);
        _owner.ankyWander.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        foreach (Transform i in _owner.fov.visibleTargets)
        {
            if (i.tag == "Rapty")
            {
                _owner.stateMachine.ChangeState(alertState.Instance);
            }
        }
    }
}