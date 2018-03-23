using UnityEngine;
using Statestuff;

public class RaptyAttackState : State<MyRapty>
{
    private static RaptyAttackState _instance;
    private float timer;

    private RaptyAttackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptyAttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptyAttackState();
            }
            return _instance;
        }
    }
    void takeDamage(MyRapty _owner)
    {
        _owner.health -= Time.deltaTime;

    }

    public override void EnterState(MyRapty _owner)
    {
        _owner.anim.SetBool("isAttacking", true);
        Debug.Log("entering Attack state");

        timer = 0;
    }

    public override void ExitState(MyRapty _owner)
    {
        _owner.anim.SetBool("isAttacking", false);
        Debug.Log("exiting AttackState");
    }

    public override void UpdateState(MyRapty _owner)
    {
        if (_owner.Enemies.Count > 0)
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                _owner.stateMachine.ChangeState(HuntState.Instance);
            }


        }
        if (_owner.Enemies.Count == 0)
        {
            _owner.stateMachine.ChangeState(HuntState.Instance);
        }



    }
}
