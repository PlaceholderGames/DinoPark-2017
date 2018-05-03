using UnityEngine;
using Statestuff;

public class EatingState : State<MyAnky>
{
    private static EatingState _instance;

    private EatingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static EatingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Eating State");
        _owner.anim.SetBool("isEating", true);
        _owner.wanderScript.enabled = false;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Eating State");
        _owner.anim.SetBool("isEating", false);
        _owner.wanderScript.enabled = true;
    }

    public override void UpdateState(MyAnky _owner)
    {

        _owner.sustenance += (Time.deltaTime * 1);

        ////////
        //State Changes
        ////////
        if (_owner.RaptorsInView.Count > 0 && _owner.sustenance > 85)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
        }

        if (_owner.RaptorsInView.Count <= 0 && _owner.sustenance > 90)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.GRAZING;
        }

    }
}
