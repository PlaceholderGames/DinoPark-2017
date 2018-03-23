using UnityEngine;
using FiniteStateMachine;

public class Alerted : State<Ankylosaurus> // This is for heightened awareness, such as looking around
{
    private static Alerted _instance;

    private Alerted()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static Alerted Instance
    {
        get
        {
            if (_instance == null)
                new Alerted();

            return _instance;
        }
    }

    public override void Enter(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isAlerted", true);
    }

    public override void Exit(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isAlerted", false);
    }

    public override void Update(Ankylosaurus parent)
    {

        if (parent.Predators.Count == 0)
            parent.State.Change(Grazing.Instance);

        if (parent.Predators.Count == 0)
        {
            parent.GetClosestPredator();
            parent.State.Change(Attacking.Instance);
        }

        if (parent.Predators.Count >= 2)
            parent.State.Change(Fleeing.Instance);
    }
}