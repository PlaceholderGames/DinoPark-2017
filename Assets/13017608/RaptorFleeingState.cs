using UnityEngine;
using Statestuff;

public class RaptorFleeingState : State<MyRapty>
{
    private static RaptorFleeingState _instance;

    private RaptorFleeingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptorFleeingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptorFleeingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering Second state");
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting Second state");
    }

    public override void UpdateState(MyRapty _owner)
    {

    }
}
