using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDrinkingState : MyState<MyAnky> {

    private static MyDrinkingState _instance;
    public MyDrinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static MyDrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new MyDrinkingState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enters DRINKING");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exits DRINKING");
    }
    public override void UpdateState(MyAnky _owner)
    {
        _owner.wander.enabled = false;
        _owner.drinking.enabled = true;
        _owner.drinking.Drink();
        //Alerted
        foreach (Transform animal in _owner.alerted.visibleTargets)
        {
            if (animal.tag == "Rapty")
            {
                _owner.mySM.ChangeState(MyAlertedState.Instance);
                return;
            }
        }
        //Idle
        if(_owner.health >= 100)
        {
            _owner.mySM.ChangeState(MyIdleState.Instance);
            return;
        }
    }
}
