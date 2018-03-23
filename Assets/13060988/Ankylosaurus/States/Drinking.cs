using UnityEngine;
using FiniteStateMachine;

public class Drinking : State<Ankylosaurus>
{
    private static Drinking _instance;

    private float _time;
    private float _tick = 0.5f;
    private float _regeneration = 1.75f;

    private Drinking()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static Drinking Instance
    {
        get
        {
            if (_instance == null)
                new Drinking();

            return _instance;
        }
    }

    public override void Enter(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isDrinking", true);
    }

    public override void Exit(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isDrinking", false);
    }

    public override void Update(Ankylosaurus parent)
    {
        if (parent.Predators.Count != 0)
            parent.State.Change(Alerted.Instance);
        else
        {
            if (parent.Thirst > 100.0f)
            {
                parent.Thirst = 100.0f;
                parent.State.Change(Grazing.Instance);
            }

            if (_time > _tick)
            {
                _time = 0.0f;
                parent.Thirst += _regeneration;
            }
        }

        _time += Time.deltaTime;
    }
}