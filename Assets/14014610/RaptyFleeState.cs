using UnityEngine;
using Statestuff;

public class raptyFleeState : State<MyRapty>
{
    private static raptyFleeState _instance;
    private raptyFleeState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static raptyFleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new raptyFleeState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering graze state");
        _owner.currentraptyState = MyRapty.raptyState.FLEEING;
        _owner.anim.SetBool("isFleeing", true);
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting graze state");
    }

    public override void UpdateState(MyRapty _owner)
    {

    }
}