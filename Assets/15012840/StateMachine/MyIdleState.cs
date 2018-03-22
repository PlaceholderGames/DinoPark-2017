using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyIdleState : MyState<MyAnky> {

    private static MyIdleState _instance;
    public MyIdleState()
    {
        if(_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static MyIdleState Instance
    {
        get
        {
            if(_instance == null)
            {
                return new MyIdleState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enters IDLE");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exits IDLE");
    }
    public override void UpdateState(MyAnky _owner)
    {
        _owner.flee.enabled = false;
        _owner.wander.enabled = true;
        //Alerted
        foreach (Transform animal in _owner.alerted.visibleTargets)
        {
            if (animal.tag == "Rapty")
            {
                _owner.mySM.ChangeState(MyAlertedState.Instance);
                return;
            }
        }
        //Drinking
        if (_owner.transform.position.y < 35 && _owner.health < 80)
        {
            _owner.mySM.ChangeState(MyDrinkingState.Instance);
            return;
        }
        //Herding
        if (_owner.alerted.visibleTargets.Count == 1)
        {
            if (_owner.herding.visibleTargets.Count > 1)
            {
                foreach (Transform item in _owner.herding.visibleTargets)
                {
                    if (item.gameObject.tag == "Rapty")
                        return;
                }
                _owner.aS.target = _owner.herding.visibleTargets[0].gameObject;
                _owner.mySM.ChangeState(MyHerdingState.Instance);
                return;
            }
        }
    }
}
