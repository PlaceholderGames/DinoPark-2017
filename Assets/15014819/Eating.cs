
using UnityEngine;
using Statedino;


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

    public static EatingState instance
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

    public override void Enterstate(MyAnky _onwer)
    {
        Debug.Log("Entering the first state");
    }

    public override void Exitstate(MyAnky _owner)
    {
        Debug.Log("Exiting the first state");
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.switchState)
        {
            _owner.Statemachine.ChangeState(AlertState.instance);
        }
        if (_owner.switchState)
        {
            _owner.Statemachine.ChangeState(GrazingState.instance);
        }
       
    }
}
