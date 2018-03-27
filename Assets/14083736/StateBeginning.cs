using UnityEngine;
using StateMachineBase;


public class StateBeginning : State<MyAnky>
{

    private static StateBeginning _instance;

    private StateBeginning()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateBeginning Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateBeginning();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("Entering BEGINNING/IDLE state");
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("Exiting BEGINNING/IDLE state");
    }

    public override void UpdateState(MyAnky _Owner)
    {
        Debug.Log("Anky has woken up! Starting walk....");
        _Owner.ankyWandering.enabled = true;
        _Owner.stateMachine.ChangeState(StateWalking.Instance);
    }
}