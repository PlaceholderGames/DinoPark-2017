using UnityEngine;
using Statestuff;

public class GrazeState : State<MyAnky>
{
    private static GrazeState _instance;

    private GrazeState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static GrazeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazeState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isGrazing", true);
        Debug.Log("entering Grazingstate");
        _owner.ankyFlee.enabled = false;
        _owner.ankyWander.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isGrazing", false);
        Debug.Log("exiting GrazeState");
        _owner.ankyWander.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {

       if(_owner.Enemies.Count > 0)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }

        //for (int i = 0; i < _owner.fov.visibleTargets.Count; i++)
        //{
        //    Debug.Log("1");
        //    Transform target = _owner.fov.visibleTargets[i];
        //    if (target.tag == "Rapty")
        //    {
        //        Debug.Log("2");
        //        _owner.Enemies.Add(target);
        //        _owner.stateMachine.ChangeState(AlertState.Instance);
        //    }
        //}

    }
}
