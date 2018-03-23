using UnityEngine;
using Statestuff;

public class DeadState : State<MyAnky>
{

    private static DeadState _instance;

    private DeadState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static DeadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DeadState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Dead State");
        _owner.AnkySeek.enabled = false;
        _owner.AnkyWander.enabled = false;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Dead State");
    }

    public override void UpdateState(MyAnky _owner)
    {
    }
}