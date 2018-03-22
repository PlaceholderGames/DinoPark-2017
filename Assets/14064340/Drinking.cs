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
        _owner.ankySeek.target = _owner.Water;
        _owner.anim.SetBool("isDrinking", true);
        Debug.Log("entering DrinkingState");
        _owner.ankySeek.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isDrinking", false);
        Debug.Log("exiting DrinkingState");
        _owner.ankySeek.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
       
        if (_owner.transform.position.y < 36)
        {
            _owner.ankySeek.enabled = false;
            _owner.hydration += (Time.deltaTime * 3) * 1.0;
        }
        if(_owner.hydration>=100)
        {
            _owner.stateMachine.ChangeState(GrazeState.Instance);
        }
   
    }
}
