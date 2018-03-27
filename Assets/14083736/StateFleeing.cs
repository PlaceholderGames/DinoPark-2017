using UnityEngine;
using StateMachineBase;


public class StateFleeing : State<MyAnky>
{

    private static StateFleeing _instance;

    private StateFleeing()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateFleeing Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateFleeing();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("Entering FLEEING state");
        _Owner.anim.SetBool("isFleeing", true);
        _Owner.ankyFleeing.enabled = true;
        _Owner.ankyWandering.enabled = false;
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("Exiting FLEEING state");
        _Owner.anim.SetBool("isFleeing", false);
        _Owner.ankyFleeing.enabled = false;
        _Owner.ankyWandering.enabled = true;
    }

    public GameObject priorityTarget = new GameObject();
    public override void UpdateState(MyAnky _Owner)
    {
        foreach(Transform i in _Owner.ankyEnemies)
        {
            
            Vector3 Difference = new Vector3();
            Vector3 RaptorDiff = new Vector3();

            if (Difference.magnitude < RaptorDiff.magnitude)
            {
                priorityTarget = i.gameObject;
            }

            Difference = (_Owner.transform.position - i.position);
            RaptorDiff = (_Owner.transform.position - priorityTarget.transform.position);

            float check = Vector3.Distance(i.transform.position, _Owner.transform.position);

            if (check > 55)
            {
                Debug.Log("Anky's gotten far enough away!");
                _Owner.stateMachine.ChangeState(StateWalking.Instance);
            }


            if (priorityTarget)
            {
                _Owner.ankyFleeing.target = priorityTarget;
            }

        }

    }
}