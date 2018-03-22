using UnityEngine;
using StateStuff;

public class AttackingState : State<AI>
{
    private static AttackingState _instance;

    private AttackingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AttackingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackingState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        _owner.agent.maxSpeed = 20;
        _owner.removeHunger = 4;
        Debug.Log("Entering Attacking State");
    }

    public override void ExitState(AI _owner)
    {
        _owner.pursue.target = _owner.prey;
        _owner.pursue.targetAux = _owner.prey;
        _owner.pursue.targetAgent = _owner.agent;
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.hunger <= 0 || _owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }

        foreach (Transform i in _owner.view.visibleTargets)
        {
            if (i.tag == "Anky" && Vector3.Distance(_owner.transform.position, i.transform.position) > 35)
            {
                _owner.prey = i.gameObject;
                _owner.stateMachine.ChangeState(HuntingState.Instance);
            }
        }
    }
}