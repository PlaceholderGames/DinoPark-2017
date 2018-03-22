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
        //_owner.dinoWander.enabled = false;
        //_owner.dinoView.enabled = false;
    }

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
    }
}
