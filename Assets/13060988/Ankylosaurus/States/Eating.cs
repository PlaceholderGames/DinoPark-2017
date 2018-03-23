using UnityEngine;
using FiniteStateMachine;

public class Eating : State<Ankylosaurus> // This is for eating depending on y value of the object to denote grass level
{
    private static Eating _instance;

    private float _time;
    private float _tick = 0.5f;
    private float _regeneration = 1.7f;

    private Eating()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static Eating Instance
    {
        get
        {
            if (_instance == null)
                new Eating();

            return _instance;
        }
    }

    public override void Enter(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isEating", true);
    }

    public override void Exit(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isEating", false);
    }

    public override void Update(Ankylosaurus parent)
    {
        if (parent.Predators.Count != 0)
            parent.State.Change(Alerted.Instance);
        else
        {
            if (parent.Hunger > 100.0f)
            {
                parent.Hunger = 100.0f;
                parent.State.Change(Grazing.Instance);
            }

            if (_time > _tick)
            {
                _time = 0.0f;
                parent.Hunger += _regeneration;
            }
        }

        _time += Time.deltaTime;
    }
}