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
        Debug.Log("idle");
        _owner.turnback.enabled = false;
        _owner.flee.enabled = false;
        _owner.seek.enabled = false;
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
        if (_owner.health < 80)
        {
            _owner.mySM.ChangeState(MyDrinkingState.Instance);
            return;
        }
        //Herding
        if (_owner.view.visibleTargets.Count == 1)
        {
            if (_owner.alerted.visibleTargets.Count > 1)
            {
                foreach (Transform item in _owner.alerted.visibleTargets)
                {
                    if (item.gameObject.tag == "Rapty")
                        return;
                }
                foreach (var dino in _owner.alerted.visibleTargets)
                {
                    if (dino.position != _owner.transform.position)
                    {
                        Debug.Log("Test");
                        _owner.mySM.ChangeState(MyHerdingState.Instance);
                        return;
                    }
                }
            }
        }
    }
}
