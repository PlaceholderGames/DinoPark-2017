using UnityEngine;
using Statestuff;

public class AlertState : State<MyAnky>
{
    private static AlertState _instance;

    private AlertState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static AlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AlertState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAlerted", true);
        Debug.Log("entering alert state");
        _owner.ankyWander.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isAlerted", false);
        Debug.Log("exiting alertState");
        _owner.ankyWander.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {

        foreach (Transform x in _owner.Enemies)
        {
      
            float Distance = Vector3.Distance(x.position, _owner.transform.position);
            if (Distance > 5 && Distance <30)
            {
                
                _owner.stateMachine.ChangeState(FleeingState.Instance);
                _owner.ankyFlee.target = x.gameObject;

            }
            else if (Distance < 5)
            {
                _owner.stateMachine.ChangeState(AttackState.Instance);
            };
        };

    }
}
