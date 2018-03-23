using UnityEngine;
using Statestuff;

public class FleeState : State<MyAnky>
{

    private static FleeState _instance;

    private FleeState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.AnkyFlee.enabled = true;
        Debug.Log("Entering Flee State");
        
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.AnkyFlee.enabled = false;
        Debug.Log("Exiting Flee State");
        
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.Enemies.Count > 0)

        foreach (Transform i in _owner.Enemies)
        {
            float Enemydistance = Vector3.Distance(i.position, _owner.transform.position);
                if (Enemydistance <= 40)
                {
                    _owner.AnkyFlee.target = i.gameObject;
                }
            if (Enemydistance >= 70)
            {
                _owner.stateMachine.ChangeState(GrazingState.Instance);
            }
        }
    }
}