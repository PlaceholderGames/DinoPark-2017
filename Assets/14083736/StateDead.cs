using UnityEngine;
using StateMachineBase;


public class StateDead : State<MyAnky>
{

    private static StateDead _instance;

    private StateDead()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateDead Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateDead();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("Entering DEAD state");
        _Owner.anim.SetBool("isDead", true);
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("Exiting DEAD state");
        _Owner.anim.SetBool("isDead", false);
    }

    public override void UpdateState(MyAnky _Owner)
    {
        //Debug.Log("Anky's dead");
    }
}