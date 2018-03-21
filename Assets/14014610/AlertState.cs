using UnityEngine;
using Statestuff;

public class alertState : State<MyAnky>
{
    private static alertState _instance;

    private alertState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static alertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new alertState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering alert state");
        _owner.anim.SetBool("isAlerted", true);
        _owner.ankyWander.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting alert state");
        _owner.anim.SetBool("isAlerted", false);
        _owner.ankyWander.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        foreach (Transform enemy in _owner.enemies)
        {
            float distance = Vector3.Distance(enemy.position, _owner.transform.position);
            if (distance < 30 && distance  > 5)
            {
                _owner.stateMachine.ChangeState(fleeState.Instance);
                _owner.ankyFlee.target = enemy.gameObject;
            }
        }

        if (_owner.enemies.Count <= 0)
        {
            _owner.stateMachine.ChangeState(grazingState.Instance);
        }


        if (_owner.hydration < 35)
        {
            _owner.stateMachine.ChangeState(drinkingState.Instance);
        }
        else if (_owner.energy < 35)
        {
            _owner.stateMachine.ChangeState(eatingState.Instance);
        }
    }
}