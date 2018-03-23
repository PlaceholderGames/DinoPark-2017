using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class HuntingState : State<MyRapty> {

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

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("Entering Hunting State");
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting Hunting State");
    }

    // -- Josh -- 
    // We call the movement function from myRapty and give it a higher value as we want to run faster
    // We keep running around the map until we find an Anky.
    public override void UpdateState(MyRapty _owner)
    {
        _owner.movement(_owner, _owner.moveSpeed * 2);

        if (_owner.dinoView.visibleTargets.Count != 0)
        {
            for (int i = 0; i < _owner.dinoView.visibleTargets.Count; i++)
            {
                if (_owner.dinoView.visibleTargets[i].CompareTag("Anky"))
                {
                    _owner.stateMachine.ChangeState(FollowingState.Instance);
                }
            }
        }
        // if our energy runs out from all the hard work, we go to the sleeping state
        if (_owner.energyLevel <= 0)
        {
            _owner.stateMachine.ChangeState(SleepingState.Instance);
        }
        // or if our water runs out, we go get a drink because you cant fight
        // with a dry mouth. 
        if (_owner.waterLevel < 20)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
        }
    }
}
