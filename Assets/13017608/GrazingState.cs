using UnityEngine;
using Statestuff;

public class GrazingState : State<MyAnky>
{
    private static GrazingState _instance;

    private GrazingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static GrazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Grazing State");
        _owner.anim.SetBool("isGrazing",true);
        _owner.wanderScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Grazing State");
        _owner.anim.SetBool("isGrazing", false);
        _owner.wanderScript.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        foreach (Transform i in _owner.fov.visibleTargets)
        {
            if (i.tag == "Rapty")
            {
                _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
        }/*
        ////////////////////////////
        //Drinking State//
        ////////////////////////////
        if (_owner.currentAnkyState == MyAnky.ankyState.DRINKING)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
        }
        ////////////////////////////
        //Eating State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.EATING)
        {
            _owner.stateMachine.ChangeState(EatingState.Instance);
        }*/
    }
}
