using UnityEngine;
using FiniteStateMachine;
using UnityEditor;

public class V_Dead : State<Velociraptor>
{
    private static V_Dead _instance;

    private V_Dead()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Dead Instance
    {
        get
        {
            if (_instance == null)
                new V_Dead();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isDead", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isDead", false);
    }

    public override void Update(Velociraptor parent)
    {
        parent.Wander.enabled = false;

        if (parent.death)
            parent.Death();
    }
}