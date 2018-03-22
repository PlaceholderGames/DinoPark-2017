using UnityEngine;
using Statestuff;

public class EatingState : State<MyAnky>
{
    private static EatingState _instance;

    private EatingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static EatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isEating", true);
        Debug.Log("entering Eating state");
        _owner.ankyWander.enabled = false;

    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isGrazing", false);
        Debug.Log("exiting EatingState");


    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.saturation += (Time.deltaTime * 2.0) * 1;
        if (_owner.saturation >= 100)
        {
            _owner.stateMachine.ChangeState(GrazeState.Instance);
        }
    }
}
