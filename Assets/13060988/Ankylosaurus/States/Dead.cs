using UnityEngine;
using FiniteStateMachine;
using UnityEditor;
using UnityEngine.Assertions.Must;

public class Dead : State<Ankylosaurus>
{
    private static Dead _instance;

    private Dead()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static Dead Instance
    {
        get
        {
            if (_instance == null)
                new Dead();

            return _instance;
        }
    }

    public override void Enter(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isDead", true);
    }

    public override void Exit(Ankylosaurus parent)
    {
    }

    public override void Update(Ankylosaurus parent)
    {
        parent.Wander.enabled = false;

        if (parent.death)
            parent.Death();
    }
}