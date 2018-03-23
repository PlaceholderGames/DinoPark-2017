using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazingState : State<MyAnky>
{


    private static GrazingState _instance;
    public GrazingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static GrazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new GrazingState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enter GRAZING");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exit GRAZING");
    }
    public override void UpdateState(MyAnky _owner)
    {

        _owner.wander.enabled = false;
        _owner.grazing.enabled = true;
        _owner.grazing.Eat();

    }
}
