using UnityEngine;
using Statestuff;

public class DeadState : State<MyAnky>
{
    private static DeadState _instance;

    private DeadState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static DeadState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DeadState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Dead State");
        _owner.anim.SetBool("isDead", true);
        //Upon Death disable all scripts
        _owner.wanderScript.enabled = false;
        _owner.seekingScript.enabled = false;
        _owner.fleeScript.enabled = false;
        _owner.tag = "Untagged";
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Dead State");
        _owner.anim.SetBool("isDead", false);
    }

    public override void UpdateState(MyAnky _owner)
    {
        //No need for an update as nothing can be brought back to life
    }
}
