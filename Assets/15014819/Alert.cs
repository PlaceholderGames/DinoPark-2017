
using UnityEngine;
using Statedino;


public class AlertState : State<MyAnky>
{
    private static AlertState _instance;

    private AlertState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AlertState instance
    {
        get
        {
            if (_instance == null)
            {
                new AlertState();
            }
            return _instance;
        }
    }

    public override void Enterstate(MyAnky _onwer)
    {
        Debug.Log("Entering the Alert state");
    }

    public override void Exitstate(MyAnky _owner)
    {
       // Debug.Log("Exiting the Alert state");
        //_owner.wander.enabled = true;
    }

    public override void Updatestate(MyAnky _owner)
    {
        if (_owner.enemyDis > 70 )
        {
            
            _owner.Statemachine.ChangeState(GrazingState.instance);
        }
        else if (_owner.enemyDis <= 70)
        {
            _owner.Statemachine.ChangeState(FleeingState.instance);
        }
        if (_owner.friendlyDis >= 70)
        {

            _owner.Statemachine.ChangeState(HerdingState.instance);
        }
       
    }
}
