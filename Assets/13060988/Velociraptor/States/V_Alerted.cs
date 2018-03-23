using UnityEngine;
using FiniteStateMachine;

public class V_Alerted : State<Velociraptor>
{
    private static V_Alerted _instance;

    private V_Alerted()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Alerted Instance
    {
        get
        {
            if (_instance == null)
                new V_Alerted();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isAlerted", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isAlerted", false);
    }

    public override void Update(Velociraptor parent)
    {
        if (parent.LivingPrey.Count == 0)
        {
            parent.State.Change(V_Hunting.Instance);
        }
        else
        {
            if (parent.Health >= 20.0f)
                parent.State.Change(V_Attacking.Instance);
            else
                parent.State.Change(V_Fleeing.Instance);
        }
    }
}