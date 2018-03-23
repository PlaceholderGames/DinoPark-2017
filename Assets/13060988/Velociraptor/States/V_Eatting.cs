using System.ComponentModel;
using UnityEngine;
using FiniteStateMachine;

public class V_Eating : State<Velociraptor>
{
    private static V_Eating _instance;

    private float _time;
    private float _tick = 0.5f;
    private float _regeneration = 3.75f;

    private V_Eating()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Eating Instance
    {
        get
        {
            if (_instance == null)
                new V_Eating();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isEating", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isEating", false);

    }

    public override void Update(Velociraptor parent)
    {

        if (parent.getClosestDeadPrey().gameObject.GetComponent<Ankylosaurus>().Meat < 0.0f)
        {
            parent.getClosestDeadPrey().gameObject.GetComponent<Ankylosaurus>().death = true;
            parent.State.Change(V_Hunting.Instance);
        }
        else
        {
            if (_time > _tick)
            {
                _time = 0.0f;
                parent.getClosestDeadPrey().gameObject.GetComponent<Ankylosaurus>().Meat -= _regeneration;
                parent.Hunger += _regeneration;

                if (parent.Hunger > 100.0f)
                    parent.Hunger = 100.0f;
            }
        }

        _time += Time.deltaTime;
    }
}