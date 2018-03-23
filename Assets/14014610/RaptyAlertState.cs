using UnityEngine;
using Statestuff;

public class raptyAlertState : State<MyRapty>
{
    private static raptyAlertState _instance;
    private raptyAlertState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static raptyAlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new raptyAlertState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyRapty _owner)
    {
        Debug.Log("entering graze state");
        _owner.currentraptyState = MyRapty.raptyState.ALERTED;
        _owner.anim.SetBool("isAlerted", true);
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isAlerted", false);
    }

    public GameObject Anky = new GameObject();

    public override void UpdateState(MyRapty _owner)
    {
        //foreach (Transform enemy in _owner.enemies)
        //{
        //    Vector3 Difference = new Vector3();
        //    Vector3 AnkyDif = new Vector3();
        //    Difference = (_owner.transform.position - enemy.position);
        //    AnkyDif = (_owner.transform.position - Anky.transform.position);

        //    if (Difference.magnitude < AnkyDif.magnitude)
        //    {
        //        Anky = enemy.gameObject;
        //    }
        //    float enemyDist = Vector3.Distance(Anky.transform.position, _owner.transform.position);
        //    _owner.raptyPursue.target = enemy.gameObject;
        //    _owner.raptyWander.enabled = false;
        //    _owner.raptyPursue.enabled = true;
        //}

        //foreach (Transform enemy in _owner.enemies)
        //{
        //    _owner.raptyPursue.target = enemy.gameObject;
        //}

        //if (_owner.enemies.Count <= 0)
        //{
        //    _owner.stateMachine.ChangeState(raptyHuntingState.Instance);
        //}

        //if (_owner.hydration < 35)
        //{
        //    _owner.stateMachine.ChangeState(raptyDrinknigState.Instance);
        //}




        //if (_owner.health <= 0)
        //{
        //    _owner.stateMachine.ChangeState(raptyDeadState.Instance);
        //}
    }
}