using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MyAlertedState : MyState<MyAnky>
{

    bool turningBack = false;
    private static MyAlertedState _instance;
    public MyAlertedState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static MyAlertedState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new MyAlertedState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enters ALERTED");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exits ALERTED");
    }
    public override void UpdateState(MyAnky _owner)
    {
        _owner.flee.enabled = false;
        _owner.seek.enabled = false;
        _owner.drinking.enabled = false;
        _owner.wander.enabled = true;

        if (!turningBack)
        {
            Transform rapty = _owner.alerted.visibleTargets.Find(animal => animal.tag == "Rapty");
            if (rapty != null)
            {
                _owner.turnback.target = rapty.gameObject;
                _owner.wander.enabled = false;
                Debug.Log("Turn back");
                _owner.turnback.enabled = true;
            }
        } 

        //Fleeing
        foreach (Transform animal in _owner.view.visibleTargets)
        {
            if (animal.tag == "Rapty")
            {
                _owner.turnback.enabled = false;
                _owner.aS.target = _owner.goal;
                _owner.mySM.ChangeState(MyFleeState.Instance);
                return;
            }
        }
        //Idle
        if (!_owner.alerted.visibleTargets.Any(animal => animal.tag == "Rapty"))
        {
            _owner.aS.target = _owner.goal;
            _owner.mySM.ChangeState(MyIdleState.Instance);
            return;
        }

    }
}
