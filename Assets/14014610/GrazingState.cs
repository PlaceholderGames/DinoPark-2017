using UnityEngine;
using Statestuff;

public class grazingState : State<MyAnky>
{
    private static grazingState _instance;

    private grazingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static grazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new grazingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering graze state");
        _owner.anim.SetBool("isGrazing", true);
        _owner.ankyWander.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isGrazing", false);
        _owner.ankyWander.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        //foreach (Transform target in _owner.fov.visibleTargets)
        //{
        //    if (target.tag == "Rapty" && !_owner.enemies.Contains(target))
        //    {
        //        _owner.enemies.Add(target);
        //    }
        //}
        //foreach (Transform target in _owner.fov.stereoVisibleTargets)
        //{
        //    if (target.tag == "Rapty" && !_owner.enemies.Contains(target))
        //    {
        //        _owner.enemies.Add(target);
        //    }
        //}

       if (_owner.enemies.Count > 0)
        {
            _owner.stateMachine.ChangeState(alertState.Instance);
        }

        if (_owner.transform.position.y < 35)
        {
            _owner.stateMachine.ChangeState(drinkingState.Instance);
        }
    }
}