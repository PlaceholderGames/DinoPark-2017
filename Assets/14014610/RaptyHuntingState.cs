using UnityEngine;
using Statestuff;

public class raptyHuntingState : State<MyRapty>
{
    private static raptyHuntingState _instance;
    private raptyHuntingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static raptyHuntingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new raptyHuntingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering graze state");
        _owner.currentraptyState = MyRapty.raptyState.HUNTING;
        _owner.anim.SetBool("isHunting", true);
        _owner.raptyWander.enabled = true;
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isHunting", false);
        _owner.raptyWander.enabled = false;
        _owner.raptySeek.enabled = false;
    }

    public GameObject Anky = new GameObject();

    public override void UpdateState(MyRapty _owner)
    {
        foreach (Transform enemy in _owner.enemies)
        {
            Vector3 Difference = new Vector3();
            Vector3 AnkyDif = new Vector3();
            Difference = (_owner.transform.position - enemy.position);
            AnkyDif = (_owner.transform.position - Anky.transform.position);

            if (Difference.magnitude < AnkyDif.magnitude)
            {
                Anky = enemy.gameObject;
            }
            float enemyDist = Vector3.Distance(Anky.transform.position, _owner.transform.position);
            _owner.raptySeek.target = enemy.gameObject;
            _owner.raptyWander.enabled = false;
            _owner.raptySeek.enabled = true;
            
            
            if (enemyDist < 5)
            {
                _owner.stateMachine.ChangeState(raptyAtackState.Instance);
            }
        }

        if (_owner.hydration < 35)
        {
            _owner.stateMachine.ChangeState(raptyDrinknigState.Instance);
        }

        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(raptyDeadState.Instance);
        }
    }
}