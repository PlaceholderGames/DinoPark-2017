using UnityEngine;
using Statestuff;

public class raptyAtackState : State<MyRapty>
{
    private static raptyAtackState _instance;
    private raptyAtackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static raptyAtackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new raptyAtackState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering graze state");
        _owner.currentraptyState = MyRapty.raptyState.ATTACKING;
        _owner.anim.SetBool("isAttacking", true);
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isAttacking", false);
    }

    public override void UpdateState(MyRapty _owner)
    {

    }
}