using UnityEngine;
using Statestuff;

public class ankyDrinkingState : State<MyAnky>
{
    private static ankyDrinkingState _instance;

    private ankyDrinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static ankyDrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ankyDrinkingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering drinking state");
        _owner.ankySeek.target = _owner.water;
        _owner.currentAnkyState = MyAnky.ankyState.DRINKING;
        _owner.anim.SetBool("isDrinking", true);
        _owner.ankySeek.enabled = true;
        
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting drinking state");
        _owner.anim.SetBool("isDrinking", false);
        _owner.ankySeek.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.transform.position.y < 36)
        {
            _owner.ankySeek.enabled = false;
            _owner.hydration += (Time.deltaTime * 1) * 1;
        }
        if (_owner.hydration >= 100)
        {
            _owner.stateMachine.ChangeState(ankyGrazingState.Instance);
        }

        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(ankyDeadState.Instance);
        }
    }
}