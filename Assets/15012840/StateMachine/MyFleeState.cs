using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFleeState : MyState<MyAnky> {

    private static MyFleeState _instance;
    public MyFleeState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static MyFleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new MyFleeState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enters FLEEING");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exits FLEEING");
    }
    public override void UpdateState(MyAnky _owner)
    {
        _owner.wander.enabled = false;
        _owner.turnback.enabled = true;
        //Idle
        foreach (Transform animal in _owner.alerted.visibleTargets)
        {
            if (animal.tag == "Rapty")
            {
                return;
            }
        }
        _owner.mySM.ChangeState(MyIdleState.Instance);
    }
}
