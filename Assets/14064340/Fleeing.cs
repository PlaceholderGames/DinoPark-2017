using UnityEngine;
using Statestuff;

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

    public static FleeingState Instance
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

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isFleeing", true);
        Debug.Log("entering Fleeing state");
        _owner.ankyFlee.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isFleeing", false);
        Debug.Log("exiting FleeingState");
        _owner.ankyFlee.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
   
     
    }
}
