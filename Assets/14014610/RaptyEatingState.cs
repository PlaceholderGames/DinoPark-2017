using UnityEngine;
using Statestuff;

public class raptyEatingState : State<MyRapty>
{
    private static raptyEatingState _instance;
    private raptyEatingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static raptyEatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new raptyEatingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering graze state");
        _owner.currentraptyState = MyRapty.raptyState.EATING;
        _owner.anim.SetBool("isEating", true);
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isEating", false);
    }

    public override void UpdateState(MyRapty _owner)
    {
        _owner.energy += (Time.deltaTime * 1) * 1;


        if (_owner.energy >= 100)
        {
            _owner.stateMachine.ChangeState(raptyHuntingState.Instance);
        }

        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(raptyDeadState.Instance);
        }
    }
}