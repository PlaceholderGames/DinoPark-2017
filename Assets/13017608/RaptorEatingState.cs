using UnityEngine;
using Statestuff;

public class RaptorEatingState : State<MyRapty>
{
    private static RaptorEatingState _instance;

    private RaptorEatingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptorEatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptorEatingState();
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
