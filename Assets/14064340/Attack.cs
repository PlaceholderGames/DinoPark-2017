using UnityEngine;
using Statestuff;

public class AttackState : State<MyAnky>
{
    private static AttackState _instance;
    private float timer;

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
    void takeDamage(MyAnky _owner)
    {
        _owner.health -= Time.deltaTime;

    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAttacking", true);
        Debug.Log("entering Attack state");
        _owner.ankyFlee.enabled = false;
        _owner.ankyWander.enabled = false;
        timer = 0;
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAttacking", false);
        Debug.Log("exiting AttackState");
    }

    public override void UpdateState(MyAnky _owner)
    {
        if(_owner.Enemies.Count > 0)
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
            

        }
        if(_owner.Enemies.Count == 0)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
       


    }
}
