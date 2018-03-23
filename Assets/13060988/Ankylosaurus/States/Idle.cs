using UnityEngine;
using FiniteStateMachine;

public class Idle : State<Ankylosaurus> // The default state on creation.
{
    private static Idle _instance;

    private Idle()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static Idle Instance
    {
        get
        {
            if (_instance == null)
                new Idle();

            return _instance;
        }
    }

    public override void Enter(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isIdle", true);
    }

    public override void Exit(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isIdle", false);
    }

    public override void Update(Ankylosaurus parent)
    {
        parent.State.Change(Grazing.Instance);
    }
}