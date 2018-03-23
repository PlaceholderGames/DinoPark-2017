using UnityEngine;
using StateStuff;

public class HuntingState : State<RaptyAI>
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

    public override void EnterState(RaptyAI _owner)
    {
        _owner.wander.enabled = false;
        _owner.removeHunger = 5;
        _owner.agent.maxSpeed = 15;

        Debug.Log("Entering Hunting State");
    }

    public override void ExitState(RaptyAI _owner)
    {
        _owner.pursue.target = _owner.prey;
        _owner.pursue.targetAux = _owner.prey;
        _owner.pursue.targetAgent = _owner.agent;
        _owner.flee.target = _owner.prey;

        Debug.Log("Exiting Hunting State");
    }

    public override void UpdateState(RaptyAI _owner)
    {
        //Sets the raptor to pursue and not flee
        
        _owner.flee.enabled = false;
        
        _owner.agent.maxSpeed = 15;

        //Check if there is an anky in range
        //Better logic is needed for attacking the closest
        foreach (Transform i in _owner.view.visibleTargets)
        {
            if (i.tag == "Anky")
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
            if (i.tag == "Anky" && Vector3.Distance(_owner.transform.position, i.transform.position) < 25
                && _owner.prey.GetComponent<AnkyAI>().health > 0)
            {
                _owner.prey = i.gameObject; //Stores seen object for A* to use
                _owner.stateMachine.ChangeState(AttackingState.Instance);
            }

        }

        //If the anky is too far away it has been lost
        if (Vector3.Distance(_owner.transform.position, _owner.prey.transform.position) > 200
            || _owner.prey.GetComponent<AnkyAI>().dead == true)
        {
            _owner.wander.enabled = true;
            _owner.pursue.enabled = false;
            _owner.enemy = false;
        } else
        {
            _owner.wander.enabled = false;
            _owner.pursue.enabled = true;
        }

        //If health is 40 or less and there is an enemy in the area then run away
        if (_owner.health <= 40 && _owner.enemy == true)
        {
            _owner.stateMachine.ChangeState(FleeingState.Instance);
        }

        //If the raptor is no longer hungry and there is no enemies it will change to idle
        if (_owner.hunger > 70 && _owner.enemy == false)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }

        if (Vector3.Distance(_owner.transform.position, _owner.prey.transform.position) < 25
            && _owner.prey.GetComponent<AnkyAI>().dead == true
            && _owner.prey.GetComponent<AnkyAI>().noMeat == false
            && _owner.hunger < 100)
        {
            _owner.stateMachine.ChangeState(EatingState.Instance);
        }
    } 
}