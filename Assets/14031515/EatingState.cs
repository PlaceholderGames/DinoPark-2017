using UnityEngine;
using Statestuff;

public class EatingState : State<MyAnky>
{

    private static EatingState _instance;

    private EatingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Eating State");
        _owner.FoodSeek.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Eating State");
        _owner.FoodSeek.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.transform.position.y > 55)
        {
            _owner.FoodSeek.enabled = false;
            _owner.Hunger++;
        }
        if (_owner.Hunger >= 100)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
        }
    }
}