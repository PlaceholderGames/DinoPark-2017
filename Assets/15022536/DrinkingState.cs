using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingState : State<MyAnky>
{
    

    private static DrinkingState _instance;
    public DrinkingState()
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
                return new DrinkingState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enter DRINKING");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exit DRINKING");
    }
    public override void UpdateState(MyAnky _owner)
    {
        
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
        if (_owner.health >= 100)
        {
            _owner.mySM.ChangeState(MyIdleState.Instance);
            return;
        }
    }
}
