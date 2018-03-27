using UnityEngine;
using StateMachineBase;


public class StateDrinking : State<MyAnky>
{

    private static StateDrinking _instance;

    private StateDrinking()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateDrinking Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateDrinking();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("Entering DRINKING state");
        _Owner.anim.SetBool("isDrinking", true);
        _Owner.anim.SetBool("isGrazing", false);
        _Owner.ankyWandering.enabled = false;
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("Exiting DRINKING state");
        _Owner.anim.SetBool("isDrinking", false);
        _Owner.anim.SetBool("isGrazing", true);
        _Owner.ankyWandering.enabled = true;
    }

    public override void UpdateState(MyAnky _Owner)
    {
        Debug.Log("Anky's drinking!");

        if (_Owner.ankyThirst != 100)
        {
            _Owner.ankyThirst = 100;
            _Owner.stateMachine.ChangeState(StateWalking.Instance);
        }
    }
}