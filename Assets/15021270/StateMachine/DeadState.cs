using UnityEngine;
using StateStuff;

public class DeadState : State<AI>
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

    public override void EnterState(AI _owner)
    {
        _owner.agent.maxSpeed = 0;
        Debug.Log("Entering Dead State");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Dead State");
    }

    public override void UpdateState(AI _owner)
    {
        //Dead!  There is no leaving this state!
    }
}