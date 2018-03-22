using UnityEngine;
using Statestuff;

public class eatingState : State<MyAnky>
{
    private static eatingState _instance;

    private eatingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static eatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new eatingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering eating state");
        _owner.currentAnkyState = MyAnky.ankyState.EATING;
        _owner.anim.SetBool("isEating", true);
        _owner.aStarScript.target = _owner.Food;
        _owner.aStarScript.enabled = true;

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting eating state");
        _owner.anim.SetBool("isEating", false);
        _owner.aStarScript.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        
        if (_owner.asPathFollowerScript.path.nodes.Count < 0 || _owner.asPathFollowerScript.path == null)
            _owner.asPathFollowerScript.path = _owner.aStarScript.path;

        _owner.move(_owner.asPathFollowerScript.getDirectionVector());
    }
}