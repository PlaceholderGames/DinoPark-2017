using UnityEngine;
using StateStuff;

public class AttackingState : State<RaptyAI>
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

    public override void EnterState(RaptyAI _owner)
    {
        _owner.agent.maxSpeed = 20;
        _owner.removeHunger = 4;
        Debug.Log("Entering Attacking State");
    }

    public override void ExitState(RaptyAI _owner)
    {
        _owner.pursue.target = _owner.prey;
        _owner.pursue.targetAux = _owner.prey;
        _owner.pursue.targetAgent = _owner.agent;
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(RaptyAI _owner)
    {
        //If there is no hunger or no health then the raptor is dead
        if (_owner.hunger <= 0 || _owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }

        if(_owner.prey.GetComponent<AnkyAI>().dead == true)
        {
            _owner.stateMachine.ChangeState(HuntingState.Instance);
        }
    }
}