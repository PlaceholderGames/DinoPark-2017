using UnityEngine;
using Statestuff;

public class GrazingState : State<MyAnky>
{

    private static GrazingState _instance;

    private GrazingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static GrazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.AnkyWander.enabled = true;
        Debug.Log("Entering Grazing State");
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.AnkyWander.enabled = false;
        Debug.Log("Exiting Grazing State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.Thirst <= 25)
        {
            _owner.stateMachine.ChangeState(DrinkState.Instance);
        }

        if (_owner.Hunger <= 25)
        {
            _owner.stateMachine.ChangeState(EatingState.Instance);
        }

        if (_owner.Enemies.Count > 0)
        {
            foreach (Transform i in _owner.Enemies)
            {
                float Enemydistance = Vector3.Distance(i.position, _owner.transform.position);
                if (Enemydistance <= 40)
                {
                    _owner.stateMachine.ChangeState(FleeState.Instance);
                }
            }
        }
    }
}