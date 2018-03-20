using UnityEngine;
using Statestuff;

public class DrinkingState : State<MyAnky>
{
    private static DrinkingState _instance;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static DrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Drinking State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Drinking State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        ////////////////////////////
        //Grazing State//
        ////////////////////////////
        if (_owner.currentAnkyState == MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
        }
        ////////////////////////////
        //Alert State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.ALERTED)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
    }
}
