using UnityEngine;
using Statestuff;

public class AttackState : State<MyAnky>
{
    private static AttackState _instance;

    private AttackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static AttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Attack State");
        _owner.anim.SetBool("isAttacking", true);
        _owner.wanderScript.enabled = false;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Attack State");
        _owner.anim.SetBool("isAttacking", false);
        _owner.wanderScript.enabled = true;
    }

    public override void UpdateState(MyAnky _owner)
    {       
        if (_owner.RaptorsInView.Count < 1)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
        }
        foreach (Transform Raptors in _owner.RaptorsInView)
        {
            float distance = Vector3.Distance(Raptors.position, _owner.transform.position);
            
            if(distance < 10)
            {
                //_owner.Attack();
                _owner.takeDamage(_owner.damage);
            }

            if (distance > 10 )
            {
                _owner.stateMachine.ChangeState(FleeingState.Instance);
                _owner.currentAnkyState = MyAnky.ankyState.FLEEING;
            }
            
        }
    }
}
