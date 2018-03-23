using UnityEngine;
using FiniteStateMachine;

public class Attacking : State<Ankylosaurus> // Causing damage to a specific target
{
    private static Attacking _instance;

    private Attacking()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static Attacking Instance
    {
        get
        {
            if (_instance == null)
                new Attacking();

            return _instance;
        }
    }

    public override void Enter(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isAttacking", true);
    }

    public override void Exit(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isAttacking", false);
    }

    public override void Update(Ankylosaurus parent)
    {
        if (parent.Health < 0.0f)
        {
            parent.Face.enabled = false;
            parent.Face.target = null;
            parent.State.Change(Dead.Instance);
        }

        if (parent.Predators.Count == 0)
        {
            parent.Face.enabled = false;
            parent.Face.target = null;
            parent.State.Change(Alerted.Instance);
        }

        if (!parent.Face.enabled)
        {
            parent.Face.enabled = true;
            parent.Face.target = parent.GetClosestPredator().gameObject;
        }
    }
}