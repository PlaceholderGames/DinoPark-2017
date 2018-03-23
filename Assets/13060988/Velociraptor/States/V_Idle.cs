using UnityEngine;
using FiniteStateMachine;

public class V_Idle : State<Velociraptor>
{
    private static V_Idle _instance;

    private V_Idle()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Idle Instance
    {
        get
        {
            if (_instance == null)
                new V_Idle();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isIdle", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isIdle", false);
    }

    public override void Update(Velociraptor parent)
    {
        parent.State.Change(V_Hunting.Instance);
    }
}