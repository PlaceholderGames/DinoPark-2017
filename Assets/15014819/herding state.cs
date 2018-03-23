
using UnityEngine;
using Statedino;


public class HerdingState : State<MyAnky>
{
    private static HerdingState _instance;

    private HerdingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static HerdingState instance
    {
        get
        {
            if (_instance == null)
            {
                new HerdingState();
            }
            return _instance;
        }
    }

    public override void Enterstate(MyAnky _onwer)
    {
        Debug.Log("entered herding");
    }

    public override void Exitstate(MyAnky _owner)
    {

    }

    public override void Updatestate(MyAnky _owner)
    {
        _owner.ankyHerd.target = _owner.Friendly;
        if (_owner.friendlyDis >= 70)
        {
            _owner.ankyHerd.enabled = true;
        }
        else if (_owner.friendlyDis < 5)
        {
            _owner.Statemachine.ChangeState(GrazingState.instance);
        }
    }
}

