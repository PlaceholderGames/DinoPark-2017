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
        _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
        _owner.anim.SetBool("isAlerted", true);
        _owner.ankyWander.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting alert state");
        _owner.anim.SetBool("isAlerted", false);
        _owner.ankyWander.enabled = false;
        _owner.ankySeek.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        foreach (Transform enemy in _owner.enemies)
        {
            float enemyDist = Vector3.Distance(enemy.position, _owner.transform.position);
            if (enemyDist < 30 && enemyDist > 5)
            {
                _owner.stateMachine.ChangeState(fleeState.Instance);
                _owner.ankyFlee.target = enemy.gameObject;
            }
        }
        foreach (Transform friend in _owner.friendlies)
        {
            float friendDist = Vector3.Distance(friend.position, _owner.transform.position);
            if (friendDist > 50)
            {
                _owner.ankySeek.target = friend.gameObject;
                _owner.ankyWander.enabled = false;
                _owner.ankySeek.enabled = true;
            }
            if (friendDist < 20)
            {
                _owner.ankySeek.enabled = false;
                _owner.ankyWander.enabled = true;
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