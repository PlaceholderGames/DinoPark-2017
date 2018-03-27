using UnityEngine;
using StateMachineBase;


public class StateAlert : State<MyAnky>
{

    private static StateAlert _instance;

    private StateAlert()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateAlert Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateAlert();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("Entering ALERT state");
        _Owner.anim.SetBool("isAlerted", true);
        _Owner.anim.SetBool("isFleeing", false);
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("Exiting ALERT state");
        _Owner.anim.SetBool("isAlerted", false);

    }

    public override void UpdateState(MyAnky _Owner)
    {
        bool ankyIsSafe = true;
        foreach (Transform i in _Owner.ankyEnemies)
        {
            float Distance = Vector3.Distance(i.position, _Owner.transform.position);

            if (Distance <= 50)
            {
                _Owner.ankyFleeing.target = i.gameObject;
                Debug.Log("Uh oh! The enemies are too close");
                ankyIsSafe = false;
;               _Owner.stateMachine.ChangeState(StateFleeing.Instance);
                
            }
        }

        Debug.Log("Finished looping!");
        if (ankyIsSafe)
        {
            Debug.Log("Anky feels safe, no longer alert!");
            _Owner.stateMachine.ChangeState(StateWalking.Instance);
            _Owner.ankyEnemies.Clear();
        }
    }
}