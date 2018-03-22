using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHerdingState : MyState<MyAnky> {
    private static MyHerdingState _instance;

    public MyHerdingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static MyHerdingState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new MyHerdingState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enters HERDING");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exits HERDING");
    }
    public override void UpdateState(MyAnky _owner)
    {
        _owner.wander.enabled = false;
        _owner.flee.enabled = true;

        if (_owner.view.visibleTargets.Count > 1)
        {
            foreach(Transform animal in _owner.view.visibleTargets)
            {
                if (animal.tag == "Anky")
                {
                    _owner.mySM.ChangeState(MyIdleState.Instance);
                    return;
                }
            }
        }

    }
}
