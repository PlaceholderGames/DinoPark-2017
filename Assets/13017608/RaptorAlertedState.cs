using UnityEngine;
using Statestuff;

public class RaptorAlertedState : State<MyRapty>
{
    private static RaptorAlertedState _instance;

    private RaptorAlertedState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static RaptorAlertedState Instance
    {
        get
        {
            if (_instance == null)
            {
                new RaptorAlertedState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("Entering Raptor Alerted State");
        _owner.aStarScript.enabled = true;
        _owner.pathFollowerScript.enabled = true;
        if (_owner.pathFollowerScript.path.nodes.Count < 1 || _owner.pathFollowerScript == null)
            _owner.pathFollowerScript.path = _owner.aStarScript.path;
        _owner.move(_owner.pathFollowerScript.getDirectionVector());

    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Raptor Alerted State");
        _owner.aStarScript.enabled = false;
        _owner.pathFollowerScript.enabled = false;
    }

    public GameObject Anky = new GameObject();

    public override void UpdateState(MyRapty _owner)
    {
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

            if (distance < 15)
            {
                _owner.stateMachine.ChangeState(RaptorAttackState.Instance);
            } 
        }
    }
}
