using UnityEngine;
using Statestuff;

public class DrinkingState : State<MyAnky>
{
    private static DrinkingState _instance;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static DrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Drinking State");
        _owner.anim.SetBool("isDrinking", true);
        _owner.seekingScript.target = _owner.Water;
        _owner.seekingScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Drinking State");
        _owner.anim.SetBool("isDrinking", false);
        _owner.seekingScript.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        if(_owner.transform.position.y < 36)
        {
            _owner.seekingScript.enabled = false;
            if (_owner.hydration <100)
                _owner.hydration += (Time.deltaTime*0.5);
        }

        ////////
        //State Changes
        ////////
        if(_owner.RaptorsInView.Count >0 && _owner.hydration > 90)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
        }

        if(_owner.RaptorsInView.Count <= 0 && _owner.hydration > 100)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.GRAZING;
        }


    }
}
