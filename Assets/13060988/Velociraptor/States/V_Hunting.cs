using UnityEngine;
using FiniteStateMachine;

public class V_Hunting : State<Velociraptor>
{
    private static V_Hunting _instance;

    private V_Hunting()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Hunting Instance
    {
        get
        {
            if (_instance == null)
                new V_Hunting();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isHunting", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isHunting", false);
    }

    public override void Update(Velociraptor parent)
    {
        if (parent.DeadPrey.Count > 0)
        {
            parent.Wander.enabled = false;
            parent.Wander.target = null;

            if (Vector3.Distance(parent.transform.position, parent.getClosestDeadPrey().transform.position) < 10.0f)
            {
                parent.Arrive.target = null;
                parent.Arrive.enabled = false;
                parent.State.Change(V_Eating.Instance);
            }
            else
            {
                if (!parent.Arrive.enabled)
                {
                    parent.Arrive.target = parent.getClosestDeadPrey().gameObject;
                    parent.Arrive.enabled = true;
                }
            }
        }
        else if (parent.LivingPrey.Count > 0)
        {
            parent.Wander.enabled = false;
            parent.Wander.target = null;
            parent.State.Change(V_Alerted.Instance);
        }
        else
        {
            if (!parent.Wander.enabled)
                parent.Wander.enabled = true;
        }
    }
}