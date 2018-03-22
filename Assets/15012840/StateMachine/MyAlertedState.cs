using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAlertedState : MyState<MyAnky>
{
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

    }
}
