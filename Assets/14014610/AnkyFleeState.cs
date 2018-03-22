using UnityEngine;
using Statestuff;

public class ankyFleeState : State<MyAnky>
{
    private static ankyFleeState _instance;

    private ankyFleeState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static ankyFleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ankyFleeState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering flee state");
        _owner.currentAnkyState = MyAnky.ankyState.FLEEING;
        _owner.anim.SetBool("isFleeing", true);
        _owner.ankySeek.enabled = false;
        _owner.ankyFlee.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting flee state");
        _owner.anim.SetBool("isFleeing", false);
        _owner.ankyFlee.enabled = false;
    }

    
    public GameObject Raptor = new GameObject();

    public override void UpdateState(MyAnky _owner)
    {
        foreach (Transform enemy in _owner.enemies)
        {
            Vector3 Difference = new Vector3();
            Vector3 RaptorDif = new Vector3();
            Difference = (_owner.transform.position - enemy.position);
            RaptorDif = (_owner.transform.position - Raptor.transform.position);

            if (Difference.magnitude < RaptorDif.magnitude)
            {
                Raptor = enemy.gameObject;
            }
            float enemyDist = Vector3.Distance(Raptor.transform.position, _owner.transform.position);
            if (enemyDist > 50)
            {
                _owner.enemies.Clear();
                _owner.stateMachine.ChangeState(ankyAlertState.Instance);
            }
            if (enemyDist < 5)
            {
                _owner.stateMachine.ChangeState(ankyAttackingState.Instance);
            }
        }

        if (Raptor)
        {
            _owner.ankyFlee.target = Raptor;
        }

        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(ankyDeadState.Instance);
        }

    }
}