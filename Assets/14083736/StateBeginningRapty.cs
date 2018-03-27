using UnityEngine;
using StateMachineBase;


public class StateBeginningRapty : State<MyRapty>
{

    private static StateBeginningRapty _instance;

    private StateBeginningRapty()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateBeginningRapty Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateBeginningRapty();
            }

            return _instance;
        }
    }

    public override void EnterState(MyRapty _Owner)
    {
        Debug.Log("Entering BEGINNING/IDLE state FOR RAPTY");
    }

    public override void ExitState(MyRapty _Owner)
    {
        Debug.Log("Exiting BEGINNING/IDLE stat FOR RAPTYe");
    }

    public override void UpdateState(MyRapty _Owner)
    {
        //Debug.Log("Rapty has woken up! Starting walk....");
    }
}