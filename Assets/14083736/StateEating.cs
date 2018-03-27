using UnityEngine;
using StateMachineBase;


public class StateEating : State<MyAnky>
{

    private static StateEating _instance;

    private StateEating()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateEating Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateEating();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("Entering EATING state");
        _Owner.anim.SetBool("isEating", true);
        _Owner.ankyWandering.enabled = false;
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("Exiting EATING state");
        _Owner.anim.SetBool("isEating", false);
        _Owner.ankyWandering.enabled = true;
    }

    public override void UpdateState(MyAnky _Owner)
    {
        Debug.Log("Anky's eating!");

        if (_Owner.ankyHunger != 100)
        {
            _Owner.ankyHunger = 100;
            _Owner.stateMachine.ChangeState(StateWalking.Instance);
        }
    }
}