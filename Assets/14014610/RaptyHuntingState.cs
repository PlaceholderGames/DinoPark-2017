using UnityEngine;
using Statestuff;

public class raptyHuntingState : State<MyRapty>
{
    private static raptyHuntingState _instance;
    private raptyHuntingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static raptyHuntingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new raptyHuntingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering graze state");
        _owner.currentraptyState = MyRapty.raptyState.HUNTING;
        _owner.anim.SetBool("isHunting", true);
        _owner.raptyWander.enabled = true;
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isHunting", false);
        _owner.raptyWander.enabled = false;
    }

    public override void UpdateState(MyRapty _owner)
    {
        //if (_owner.enemies.Count > 0)
        //{
        //    _owner.stateMachine.ChangeState(raptyAlertState.Instance);
        //}
    }
}