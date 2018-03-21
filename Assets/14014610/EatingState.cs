using UnityEngine;
using Statestuff;

public class eatingState : State<MyAnky>
{
    private static eatingState _instance;

    private eatingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static eatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new eatingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering eating state");
        _owner.currentAnkyState = MyAnky.ankyState.EATING;
        _owner.anim.SetBool("isEating", true);

    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting eating state");
        _owner.anim.SetBool("isEating", false);
    }

    public override void UpdateState(MyAnky _owner)
    {

    }
}