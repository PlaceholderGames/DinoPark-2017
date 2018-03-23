using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedState : State<MyAnky>
{
    private static AlertedState _instance;
    public AlertedState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static AlertedState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new AlertedState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enter ALERTED");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exit ALERTED");
    }
    public override void UpdateState(MyAnky _owner)
    {

    }
}
