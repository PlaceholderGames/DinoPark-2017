using UnityEngine;
using FiniteStateMachine;

public class V_Drinking : State<Velociraptor>
{
    private static V_Drinking _instance;

    private float _time;
    private float _tick = 0.5f;
    private float _regeneration = 1.75f;

    private V_Drinking()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Drinking Instance
    {
        get
        {
            if (_instance == null)
                new V_Drinking();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isDrinking", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isDrinking", false);
    }

    public override void Update(Velociraptor parent)
    {
        if (parent.Thirst > 100.0f)
        {
            parent.Thirst = 100.0f;
            parent.State.Change(V_Hunting.Instance);
        }
        else
        {
            if (_time > _tick)
            {
                _time = 0.0f;

                parent.Thirst += _regeneration;
            }
        }

        _time += Time.deltaTime;
    }
}