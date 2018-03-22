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
        //Sets the raptor to pursue and not flee
        _owner.flee.enabled = false;
        _owner.pursue.enabled = true;

        //Check if there is an anky in range
        //Better logic is needed for attacking the closest
        foreach (Transform i in _owner.view.visibleTargets)
        {
            if(i.tag == "Anky")
            {
                _owner.enemy = true;
                _owner.prey = i.gameObject; //Stores seen object for A* to use
            }
            else
            {
                _owner.enemy = false;
            }

            //If close enough to the anky it will switch to attack
            //In this state it can no longer flee and is faster
            //This will stop it running away because it is too desprate for food
            if (i.tag == "Anky" && Vector3.Distance(_owner.transform.position, i.transform.position) < 25)
            {
                _owner.prey = i.gameObject; //Stores seen object for A* to use
                _owner.stateMachine.ChangeState(AttackingState.Instance);
            }
        }

        //If health is 40 or less and there is an enemy in the area then run away
        if (_owner.health <= 40 && _owner.enemy == true)
        {
                _owner.stateMachine.ChangeState(FleeingState.Instance);
        }

        //If the raptor is no longer hungry and there is no enemies it will change to idle
        if(_owner.hunger > 70 && _owner.enemy == false)
        {
             _owner.stateMachine.ChangeState(IdleState.Instance);
        }

        //_owner.stateMachine.ChangeState(EatingState.Instance);

        //
    }
}