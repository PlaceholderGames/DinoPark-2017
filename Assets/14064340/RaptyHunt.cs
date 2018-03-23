using UnityEngine;
using Statestuff;

public class HuntState : State<MyRapty>
{
    private static HuntState _instance;

    private HuntState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static HuntState Instance
    {
        get
        {
            if (_instance == null)
            {
                new HuntState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
       // _owner.anim.SetBool("isDead", true);
        Debug.Log("entering hunt state");
        _owner.RaptySeek.enabled = true;
 
    }

    public override void ExitState(MyRapty _owner)
    {
       // _owner.anim.SetBool("isDead", false);
        Debug.Log("exiting huntState");
    }

    public override void UpdateState(MyRapty _owner)
    {

    }
}
