using UnityEngine;
using StateStuff;

public class DrinkingState : State<AI>
{
    private float addThirstTimer;

    private static DrinkingState _instance;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static DrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {

        Debug.Log("Entering Drinking State");
    }

    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting Drinking State");
    }

    public override void UpdateState(AI _owner)
    {
        //Drink until the raptor has 100 thrist and then switch to idle
        addThirstTimer += Time.deltaTime;
        if (addThirstTimer >= 1)
        {
            _owner.thirst += 10;
            addThirstTimer = 0;
        }
        
        if(_owner.thirst >= 100)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }
        
    }
}