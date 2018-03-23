
using UnityEngine;
using Statedino;


public class FirstState : State<MyAnky>
{
    private static FirstState _instance;

    private FirstState ()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FirstState instance
    {
        get
        {
            if (_instance == null)
            {
                new FirstState();
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
            _owner.Statemachine.ChangeState(SecondState.instance);
        }
    }
}
