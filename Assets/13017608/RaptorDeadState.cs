using UnityEngine;
using Statestuff;

public class RaptorDeadState : State<MyRapty>
{
    private static RaptorDeadState _instance;

    private RaptorDeadState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptorDeadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptorDeadState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("Entering Dead State");
        _owner.aStarScript.enabled = false;
        _owner.pathFollowerScript.enabled = false;
        _owner.wanderScript.enabled = false;
        _owner.facingScript.enabled = false;
        _owner.aStarScript.path = null;
        _owner.pathFollowerScript.path = null;
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Dead State");
    }

    public override void UpdateState(MyRapty _owner)
    {

    }
}
