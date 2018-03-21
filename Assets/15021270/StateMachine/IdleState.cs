using UnityEngine;
using StateStuff;

public class IdleState : State<AI>
{

    private float healthTimer;

    private static IdleState _instance;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        //_owner.pursue.enabled = false;
        _owner.agent.maxSpeed = 3;
        _owner.removeHunger = 10;
        _owner.flee.target = _owner.gameObject;

        

        Debug.Log("Entering idle State");
    }

    public override void ExitState(AI _owner)
    {
        _owner.pursue.target = _owner.prey;
        _owner.pursue.targetAux = _owner.prey;
        _owner.pursue.targetAgent = _owner.agent;
        _owner.flee.target = _owner.prey;

        Debug.Log("Exiting idle State");
    }

    public override void UpdateState(AI _owner)
    {

        _owner.pursue.enabled = false;
        _owner.wander.enabled = true;

        //While in idle health regenerates over time
        //This means 
        healthTimer += Time.deltaTime;
        if (healthTimer >= 1 && _owner.health <= 100)
        {
            healthTimer = 0;
            _owner.health += 1;
        }

        //Logic for patrolling and searching for a raptor
        if (_owner.hunger <= 70)
        {
            _owner.stateMachine.ChangeState(HuntingState.Instance);
        }

        //Check if there is an anky in range
        //Better logic is needed for attacking the closest
        foreach (Transform i in _owner.view.visibleTargets)
        {
            if (i.tag == "Anky")
            {
                _owner.prey = i.gameObject;
                Debug.Log(_owner.prey);
                _owner.stateMachine.ChangeState(HuntingState.Instance);
            }
        }
        
    }
}