using UnityEngine;
using Statestuff;

public class attackingState : State<MyAnky>
{
    private static attackingState _instance;

    private attackingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static attackingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new attackingState();
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

    }
}