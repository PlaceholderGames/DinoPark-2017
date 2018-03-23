using UnityEngine;
using Statestuff;

public class ankyAttackingState : State<MyAnky>
{
    private static ankyAttackingState _instance;

    private ankyAttackingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static ankyAttackingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ankyAttackingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering attack state");
        _owner.currentAnkyState = MyAnky.ankyState.ATTACKING;
        _owner.anim.SetBool("isAttacking", true);

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting attack state");
        _owner.anim.SetBool("isAttacking", false);
    }

    

    public override void UpdateState(MyAnky _owner)
    {
        foreach (Transform enemy in _owner.enemies)
        {
            float enemyDist = Vector3.Distance(enemy.position, _owner.transform.position);
            if (enemyDist > 5)
            {
                _owner.stateMachine.ChangeState(ankyGrazingState.Instance);
            }
        }
        
        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(ankyDeadState.Instance);
        }
    }
}