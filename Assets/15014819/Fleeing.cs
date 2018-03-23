
using UnityEngine;
using Statedino;


public class FleeingState : State<MyAnky>
{
    private static FleeingState _instance;

    private FleeingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FleeingState instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeingState();
            }
            return _instance;
        }
    }

    public override void Enterstate(MyAnky _onwer)
    {
        Debug.Log("Entering the fleeing state");
    }

    public override void Exitstate(MyAnky _owner)
    {
        Debug.Log("Exiting the fleeing state");
    }

    public override void Updatestate(MyAnky _owner)
    {


        if (_owner.enemyDis <= 70)
        {
            _owner.fleeAnky.enabled = true;
        }
        else if (_owner.enemyDis > 70)
        {
            _owner.Statemachine.ChangeState(GrazingState.instance);
        }

    }
}
