
using UnityEngine;
using Statedino;


public class SecondState : State<MyAnky>
{
    private static SecondState _instance;

    private SecondState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static SecondState instance
    {
        get
        {
            if (_instance == null)
            {
                new SecondState();
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
        if (!_owner.switchState)
        {
            _owner.Statemachine.ChangeState(FirstState.instance);
        }
    }
}
