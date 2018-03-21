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
        _owner.pursue.enabled = true;
        _owner.removeHunger = 5;
        _owner.agent.maxSpeed = 15;

        Debug.Log("Entering Hunting State");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Hunting State");
    }

    public override void UpdateState(AI _owner)
    {
        //Update while in hunting state
        //_owner.SetSteering(_owner.face.GetSteering());
        //_owner.SetSteering(_owner.pursue.GetSteering());

        //Check if there is an anky in range
        //Better logic is needed for attacking the closest
        foreach (Transform i in _owner.view.visibleTargets)
        {
            //Debug.Log(Vector3.Distance(_owner.transform.position, i.transform.position));
            if (i.tag == "Anky" && Vector3.Distance(_owner.transform.position, i.transform.position) < 20)
            {
                _owner.stateMachine.ChangeState(AttackingState.Instance);
            }
        }

        //_owner.stateMachine.ChangeState(EatingState.Instance);

        //_owner.stateMachine.ChangeState(DrinkingState.Instance);

        //_owner.stateMachine.ChangeState(FleeingState.Instance);

        //_owner.stateMachine.ChangeState(IdleState.Instance);
    }
}