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
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Eating State");
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
