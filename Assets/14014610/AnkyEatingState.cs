using UnityEngine;
using Statestuff;

public class ankyEatingState : State<MyAnky>
{
    private static ankyEatingState _instance;

    private ankyEatingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static ankyEatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ankyEatingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering eating state");
        _owner.currentAnkyState = MyAnky.ankyState.EATING;
        _owner.anim.SetBool("isEating", true);

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting eating state");
        _owner.anim.SetBool("isEating", false);
    }

    public override void UpdateState(MyAnky _owner)
    {

        //if (_owner.asPathFollowerScript.path.nodes.Count < 0 || _owner.asPathFollowerScript.path == null)
        //    _owner.asPathFollowerScript.path = _owner.aStarScript.path;
        //
        //_owner.move(_owner.asPathFollowerScript.getDirectionVector());


        _owner.energy += (Time.deltaTime * 1) * 1;
        

        if (_owner.energy >= 100)
        {
            _owner.stateMachine.ChangeState(ankyGrazingState.Instance);
        }

        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(ankyDeadState.Instance);
        }

    }
}