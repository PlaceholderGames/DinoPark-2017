using UnityEngine;
using Statestuff;

public class FleeingState : State<MyAnky>
{
    private static FleeingState _instance;

    private FleeingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static FleeingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FleeingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Fleeing State");
        _owner.anim.SetBool("isFleeing", true);
        _owner.fleeScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Fleeing State");
        _owner.anim.SetBool("isFleeing", false);
        _owner.fleeScript.enabled = false;
    }

    public GameObject Raptor = new GameObject();

    public override void UpdateState(MyAnky _owner)
    {
        foreach(Transform i in _owner.RaptorsInView)
        {
            Vector3 Difference = new Vector3();
            Vector3 RaptorDifference = new Vector3();
            Difference = (_owner.transform.position - i.position);
            RaptorDifference = (_owner.transform.position - Raptor.transform.position);

            if (Difference.magnitude < RaptorDifference.magnitude)
            {
                Raptor = i.gameObject;
            }
            float distance = Vector3.Distance(Raptor.transform.position, _owner.transform.position);
            if (distance > 50)
            {
                _owner.stateMachine.ChangeState(AlertState.Instance);
                _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
            }
            if (distance < 10)
            {
                _owner.stateMachine.ChangeState(AttackState.Instance);
                _owner.currentAnkyState = MyAnky.ankyState.ATTACKING;
            }
        }
        if (Raptor)
        {
            _owner.fleeScript.target = Raptor;
        }

    }
}
