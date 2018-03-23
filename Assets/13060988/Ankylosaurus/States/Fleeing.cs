using UnityEngine;
using FiniteStateMachine;

public class Fleeing : State<Ankylosaurus> // Running away from a specific target
{
    private static Fleeing _instance;

    private Fleeing()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static Fleeing Instance
    {
        get
        {
            if (_instance == null)
                new Fleeing();

            return _instance;
        }
    }

    public override void Enter(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isFleeing", true);
    }

    public override void Exit(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isFleeing", false);
    }

    public override void Update(Ankylosaurus parent)
    {
        if (!parent.Flee.enabled)
        {
            parent.Flee.target = parent.GetClosestPredator().gameObject;
            parent.Flee.enabled = true;
        }
        else
        {
            if (parent.Health <= 0.0f)
            {
                parent.Flee.enabled = false;
                parent.Flee.target = null;
                parent.State.Change(Dead.Instance);
            }
            else if (Vector3.Distance(parent.transform.position, parent.Flee.target.transform.position) > 150.0f)
            {
                parent.Flee.enabled = false;
                parent.Flee.target = null;
                parent.State.Change(Alerted.Instance);
            }
        }
    }
}