
using UnityEngine;
using Statedino;


public class GrazingState : State<MyAnky>
{
    private static GrazingState _instance;

    private GrazingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static GrazingState instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazingState();
            }
            return _instance;
        }
    }

    public override void Enterstate(MyAnky _onwer)
    {
        Debug.Log("Entering the Grazing state");
        _onwer.ankySeek.enabled = false;
    }

    public override void Exitstate(MyAnky _owner)
    {
        _owner.wander.enabled = false;
    }

    public override void Updatestate(MyAnky _owner)
    {

       

        if (_owner.energy <= 15)
        {
            _owner.wander.enabled = false;
            _owner.Statemachine.ChangeState(DrinkingState.instance);
        }

       
        else if (_owner.enemy != null)
        {
            Debug.Log("Bad Guy");
            _owner.Statemachine.ChangeState(AlertState.instance);
        }
        else
        {
            
            _owner.wander.enabled = true;
            _owner.ankySeek.enabled = false;
        }
        if (_owner.friendlyDis > 20)
        {
            Debug.Log("entering herding");
            _owner.Statemachine.ChangeState(HerdingState.instance);
        }



    }
}
