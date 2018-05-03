using UnityEngine;
using Statestuff;

public class RaptorDrinkingState : State<MyRapty>
{
    private static RaptorDrinkingState _instance;

    private RaptorDrinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptorDrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptorDrinkingState();
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
