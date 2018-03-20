using UnityEngine;
using Statestuff;

public class AlertState : State<MyAnky>
{
    private static AlertState _instance;

    private AlertState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static AlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AlertState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Alert State");
        _owner.wanderScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Alert State");
        _owner.wanderScript.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        foreach (Transform i in _owner.fov.visibleTargets)
        {
            if (i.tag == "Rapty" && Vector3.Distance(i.position, _owner.transform.position) < 30)
            {
                //_owner.currentAnkyState = MyAnky.ankyState.FLEEING;
                _owner.fleeScript.target = i.gameObject;
                _owner.stateMachine.ChangeState(FleeingState.Instance);
            }
        }

        ////////////////////////////
        //Grazing State//
        ////////////////////////////
       /* if (_owner.currentAnkyState == MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
        }
        ////////////////////////////
        //Drinking State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.DRINKING)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
        }
        ////////////////////////////
        //Eating State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.EATING)
        {
            _owner.stateMachine.ChangeState(EatingState.Instance);
        }
        ////////////////////////////
        //Fleeing State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.FLEEING)
        {
            _owner.stateMachine.ChangeState(FleeingState.Instance);
        }
        ////////////////////////////
        //Attacking State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.ATTACKING)
        {
            _owner.stateMachine.ChangeState(AttackState.Instance);
        }*/
    }
}
