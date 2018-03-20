using UnityEngine;
using Statestuff;

public class fleeState : State<MyAnky>
{
    private static fleeState _instance;

    private fleeState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static fleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new fleeState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering flee state");
        _owner.anim.SetBool("isFleeing", true);
        _owner.ankyFlee.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting flee state");
        _owner.anim.SetBool("isFleeing", false);
        _owner.ankyFlee.enabled = false;
    }

    public Vector3 closestEnemy = new Vector3(1000, 1000, 1000);
    public int closestenemyIndex = 0;
    public GameObject Raptor = new GameObject();

    public override void UpdateState(MyAnky _owner)
    {
        //for (int i = 0; i < _owner.enemies.Count; i++)
        //{
        //    Vector3 Difference = new Vector3();
        //    Difference = (_owner.transform.position - _owner.enemies[i].position);
        //    if (Difference.magnitude < closestEnemy.magnitude)
        //    {
        //        closestEnemy = Difference;
        //        closestenemyIndex = 1;
        //        Raptor = _owner.enemies[i].gameObject;
        //    }
        //}

        foreach (Transform enemy in _owner.enemies)
        {
            float distance = Vector3.Distance(enemy.position, _owner.transform.position);
            if (distance > 40)
            {
                _owner.stateMachine.ChangeState(alertState.Instance);
            }
        }
    }
}