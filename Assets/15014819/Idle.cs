
using UnityEngine;
using Statedino;


public class IdleState : State<MyAnky>
{
    private static IdleState _instance;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }
            return _instance;
        }
    }

    public override void Enterstate(MyAnky _onwer)
    {
        Debug.Log("Entering the Idle state");
    }

    public override void Exitstate(MyAnky _owner)
    {
        Debug.Log("Exiting the Idle state");
    }

    public override void Updatestate(MyAnky _owner)
    {
       _owner.Statemachine.ChangeState(GrazingState.instance);
        
    }
}

