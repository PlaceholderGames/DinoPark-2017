using UnityEngine;
using StateStuff;

public class EatingState : State<RaptyAI>
{
    private static EatingState _instance;

    private float addHungerTimer;

    private EatingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatingState();
            }

            return _instance;
        }
    }

    public override void EnterState(RaptyAI _owner)
    {

        Debug.Log("Entering Eating State");
    }

    public override void ExitState(RaptyAI _owner)
    {

        Debug.Log("Exiting Eating State");
    }

    public override void UpdateState(RaptyAI _owner)
    {

        _owner.wander.enabled = false;
        _owner.flee.enabled = false;
        _owner.pursue.enabled = false;
        //_owner.stateMachine.ChangeState(HuntingState.Instance);

        addHungerTimer += Time.deltaTime;
        if (addHungerTimer >= 1)
        {
            _owner.hunger += 10;
            addHungerTimer = 0;
        }

        if (_owner.hunger >= 100 || _owner.prey.GetComponent<AnkyAI>().noMeat == true)
        {
            _owner.stateMachine.ChangeState(HuntingState.Instance);
        }
    }
}