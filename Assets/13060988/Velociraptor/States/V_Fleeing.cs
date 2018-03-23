using UnityEngine;
using FiniteStateMachine;

public class V_Fleeing : State<Velociraptor> // Running away from a specific target
{
    private static V_Fleeing _instance;

    private V_Fleeing()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Fleeing Instance
    {
        get
        {
            if (_instance == null)
                new V_Fleeing();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isFleeing", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isFleeing", false);
    }

    public override void Update(Velociraptor parent)
    {
        if (parent.Health < 0.0f)
            parent.State.Change(V_Dead.Instance);
        else
        {
            if (!parent.Flee.enabled)
            {
                parent.Flee.target = parent.getClosestLivingPrey().gameObject;
                parent.Flee.enabled = true;
            }
            else if (Vector3.Distance(parent.transform.position, parent.Flee.target.transform.position) > 300.0f)
            {
                parent.Flee.enabled = false;
                parent.Flee.target = null;
                parent.State.Change(V_Hunting.Instance);
            }
        }
    }
}