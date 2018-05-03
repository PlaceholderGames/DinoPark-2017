using UnityEngine;
using Statestuff;

public class RaptorAttackState : State<MyRapty>
{
    private static RaptorAttackState _instance;

    private RaptorAttackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptorAttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptorAttackState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("Entering Raptor Attack State");
        _owner.aStarScript.enabled = false;
        _owner.pathFollowerScript.enabled = false;
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Raptor Attack State");
    }
    public GameObject Anky = new GameObject();
    public override void UpdateState(MyRapty _owner)
    {
        if (_owner.AnkyInView.Count < 1)
        {
            _owner.stateMachine.ChangeState(RaptorHuntingState.Instance);
        }
        foreach (Transform i in _owner.AnkyInView)
        {
            Vector3 Difference = new Vector3();
            Vector3 AnkyDifference = new Vector3();
            Difference = (_owner.transform.position - i.position);
            AnkyDifference = (_owner.transform.position - Anky.transform.position);

            if (Difference.magnitude < AnkyDifference.magnitude)
            {
                Anky = i.gameObject;
            }
            float distance = Vector3.Distance(Anky.transform.position, _owner.transform.position);

            if (distance > 15)
            {
                _owner.stateMachine.ChangeState(RaptorAlertedState.Instance);
            }
        }
    }
}
