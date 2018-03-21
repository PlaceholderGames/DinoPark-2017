using UnityEngine;
using StateStuff;

public class HuntingState : State<AI>
{

    private static HuntingState _instance;

    private HuntingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static HuntingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new HuntingState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        _owner.wander.enabled = false;
        //_owner.pursue.enabled = true;
        _owner.removeHunger = 5;
        _owner.agent.maxSpeed = 15;


        Debug.Log("Entering Hunting State");
    }

    public override void ExitState(AI _owner)
    {
        _owner.pursue.target = _owner.prey;
        _owner.pursue.targetAux = _owner.prey;
        _owner.pursue.targetAgent = _owner.agent;
        _owner.flee.target = _owner.prey;

        Debug.Log("Exiting Hunting State");
    }

    public override void UpdateState(AI _owner)
    {
        //Updates while state is hunting
        _owner.pursue.enabled = true;
        _owner.flee.enabled = false;

        //Check if there is an anky in range
        //Better logic is needed for attacking the closest
        foreach (Transform i in _owner.view.visibleTargets)
        {
            if(i.tag == "Anky")
            {
                _owner.enemy = true;
                _owner.prey = i.gameObject;
            }
            else
            {
                _owner.enemy = false;
            }

            //Debug.Log(Vector3.Distance(_owner.transform.position, i.transform.position));
            if (i.tag == "Anky" && Vector3.Distance(_owner.transform.position, i.transform.position) < 25)
            {
                _owner.prey = i.gameObject;
                _owner.stateMachine.ChangeState(AttackingState.Instance);
            }
        }

        if (_owner.health <= 70 && _owner.enemy == true)
        {
                _owner.stateMachine.ChangeState(FleeingState.Instance);
        }

        if(_owner.hunger > 70 && _owner.enemy == false)
        {
             _owner.stateMachine.ChangeState(IdleState.Instance);
        }

        //_owner.stateMachine.ChangeState(EatingState.Instance);

        //_owner.stateMachine.ChangeState(DrinkingState.Instance);
    }
}