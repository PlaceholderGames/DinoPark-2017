using UnityEngine;
using StateStuff;

public class IdleState : State<RaptyAI>
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

    public override void EnterState(RaptyAI _owner)
    {
        _owner.agent.maxSpeed = 3;
        _owner.removeHunger = 10;
        _owner.flee.target = _owner.gameObject;

        

        Debug.Log("Entering idle State");
    }

    public override void ExitState(RaptyAI _owner)
    {
        _owner.pursue.target = _owner.prey;
        _owner.pursue.targetAux = _owner.prey;
        _owner.pursue.targetAgent = _owner.agent;
        _owner.flee.target = _owner.prey;

        Debug.Log("Exiting idle State");
    }

    public override void UpdateState(RaptyAI _owner)
    {
        //Stops the raptor chasing after any ankys in idle
        _owner.pursue.enabled = false;

        
            //If the raptor is thirsty it will take drinking priority over hunting
            //While in idle it means the raptor is full and that it does not need to hunt
            if (_owner.thirst < 50)
        {
            _owner.wander.enabled = true;
            _owner.Astar.target = _owner.waterLocation;
            _owner.move(_owner.follower.getDirectionVector());

            //While looking for water if the raptor enters a deep patch of water, stop to drink
            if (_owner.transform.position.y <= 35)
            {
                _owner.stateMachine.ChangeState(DrinkingState.Instance);
            }
        }
        else //After the raptor isn't thirsty it will hunt anyway, even if full
        {
            //Check if there is an anky in range
            //Better logic is needed for attacking the closest
            foreach (Transform i in _owner.view.visibleTargets)
            {
                if (i.tag == "Anky")
                {
                    _owner.prey = i.gameObject;
                    //Debug.Log(_owner.prey);

                    if (_owner.prey.GetComponent<AnkyAI>().dead == false)
                    {
                        _owner.stateMachine.ChangeState(HuntingState.Instance);
                    }
                    
                }
            }
            //_owner.wander.enabled = false;
        }

        //While in idle health regenerates over time
        //This means it is taking a break and conserving its energy
        healthTimer += Time.deltaTime;
        if (healthTimer >= 1 && _owner.health <= 100)
        {
            healthTimer = 0;
            _owner.health += 1;
        }

        //When the raptor is hungry it will change to hunting state where it is faster
        if (_owner.hunger <= 70)
        {
            _owner.stateMachine.ChangeState(HuntingState.Instance);
        }

        //If the raptor is outside of the range of the alpha it will run towards it
        if(Vector3.Distance(_owner.transform.position, _owner.friendly.transform.position) > 80 &&
            _owner.thirst >= 50)
        {
            _owner.returnToAlpha = true;
        }
        else if(Vector3.Distance(_owner.transform.position, _owner.friendly.transform.position) < 50)
        {
            _owner.returnToAlpha = false;
        }

        
        //Speed is inscreased and the path is set to move the raptor
        if (_owner.returnToAlpha)
        {
            _owner.agent.maxSpeed = 10;
            _owner.Astar.target = _owner.friendly;
            _owner.move(_owner.follower.getDirectionVector());
            _owner.wander.enabled = false;
        }
        else
        {
            _owner.agent.maxSpeed = 3;
            _owner.wander.enabled = true;
        }
            
    }
}