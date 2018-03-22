using UnityEngine;
using Statestuff;

public class raptyDrinknigState : State<MyRapty>
{
    private static raptyDrinknigState _instance;
    private raptyDrinknigState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static raptyDrinknigState Instance
    {
        get
        {
            if (_instance == null)
            {
                new raptyDrinknigState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering graze state");
        _owner.raptySeek.target = _owner.water;
        _owner.currentraptyState = MyRapty.raptyState.DRINKING;
        _owner.anim.SetBool("isDrinking", true);
        _owner.raptySeek.enabled = true;
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isDrinking", false);
        _owner.raptySeek.enabled = false;
    }

    public override void UpdateState(MyRapty _owner)
    {
        if (_owner.transform.position.y < 36)
        {
            _owner.raptySeek.enabled = false;
            _owner.hydration += (Time.deltaTime * 1) * 1;
        }
        if (_owner.hydration >= 100)
        {
            _owner.stateMachine.ChangeState(raptyHuntingState.Instance);
        }

        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(raptyDeadState.Instance);
        }
    }
}