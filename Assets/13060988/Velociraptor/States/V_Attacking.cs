using UnityEngine;
using FiniteStateMachine;

public class V_Attacking : State<Velociraptor>
{
    private static V_Attacking _instance;

    private V_Attacking()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Attacking Instance
    {
        get
        {
            if (_instance == null)
                new V_Attacking();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isAttacking", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isAttacking", false);
    }

    public override void Update(Velociraptor parent)
    {
        if (!parent.Seek.enabled)
        {
            parent.Seek.target = parent.getClosestLivingPrey().gameObject;
            parent.Seek.enabled = true;
        }
        else
        {
            if (!(parent.Seek.target.gameObject.GetComponent<Ankylosaurus>().State.CurrentState is Dead))
            {
                if (parent.Health < 0.0f)
                {
                    parent.Seek.enabled = false;
                    parent.Seek.target = null;
                    parent.State.Change(V_Dead.Instance);
                }
                else if (parent.Health < 20.0f)
                {
                    parent.Seek.enabled = false;
                    parent.Seek.target = null;
                    parent.State.Change(V_Fleeing.Instance);
                }
            }
            else
            {
                parent.Seek.enabled = false;
                parent.Seek.target = null;
                parent.State.Change(V_Hunting.Instance);
            }
        }
    }
}