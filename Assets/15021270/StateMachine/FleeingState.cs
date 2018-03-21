using UnityEngine;
using StateStuff;

public class FleeingState : State<AI>
{
    private static FleeingState _instance;

    private FleeingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FleeingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeingState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        _owner.pursue.target = _owner.prey;
        _owner.pursue.targetAux = _owner.prey;
        _owner.pursue.targetAgent = _owner.agent;
        _owner.pursue.orientation = 180;

        _owner.flee.target = _owner.prey;
        

        Debug.Log("Entering Fleeing State");
    }

    public override void ExitState(AI _owner)
    {
        _owner.enemy = false;
        _owner.fleeingTime = 0;
        _owner.pursue.orientation = 0;
        Debug.Log("Exiting Fleeing State");
    }

    public override void UpdateState(AI _owner)
    {
        _owner.pursue.enabled = false;
        _owner.flee.enabled = true;

        //_owner.agent.SetSteering(_owner.flee.GetSteering());
        //_owner.agent.SetSteering(_owner.pursue.GetSteering());

        _owner.fleeingTime += Time.deltaTime;
        if(_owner.fleeingTime >= 10)
        {
            _owner.stateMachine.ChangeState(HuntingState.Instance);
                    
        }

        if(_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }
        
    }
}