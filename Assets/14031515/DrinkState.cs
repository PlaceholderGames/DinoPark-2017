using UnityEngine;
using Statestuff;

public class DrinkState : State<MyAnky>
{

    private static DrinkState _instance;

    private DrinkState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static DrinkState Instance
    {
        get
        {
            if(_instance == null)
            {
                new DrinkState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Drink State");
        _owner.AnkySeek.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Drink State");
        _owner.AnkySeek.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.transform.position.y < 37)
        {
            _owner.AnkySeek.enabled = false;
            _owner.Thirst++;
        }
        if (_owner.Thirst >=100)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
        }
    }
}