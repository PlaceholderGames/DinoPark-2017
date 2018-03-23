using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdingState : State<MyAnky>
{


    private static HerdingState _instance;
    public HerdingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static HerdingState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new HerdingState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enter HERDING");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exit HERDING");
    }
    public override void UpdateState(MyAnky _owner)
    {
        
        _owner.SM.ChangeState(GrazingState.Instance);
       
    }
}
