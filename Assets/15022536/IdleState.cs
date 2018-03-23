using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<MyAnky>
{

    private static IdleState _instance;
    public IdleState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new IdleState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enter IdleState");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exit IdleState");
    }
    public override void UpdateState(MyAnky _owner)
    {
        _owner.flee.enabled = false;
        _owner.wander.enabled = true;
        //switch to Alerted State as an "exit" way in case of a danger withing certian range
        foreach (Transform animal in _owner.AnkyAlerted.visibleTargets)
        {
            if (animal.tag == "Rapty")
            {
                _owner.SM.ChangeState(AlertedState.Instance);
                return;
            }
        }
        //change to DrinkingState if below certian y pos and Anky water level gets below 50
        if (_owner.transform.position.y < 36 && _owner.water < 50)
        {
            _owner.SM.ChangeState(DrinkingState.Instance);
            return;
        }
        //change to Grazing 
        if (_owner.transform.position.y > 56 && _owner.food < 50)
        {
            _owner.SM.ChangeState(GrazingState.Instance);
            return;
        }

        //change to HerdingState if there are no predators around
        if (_owner.AnkyAlerted.visibleTargets.Count == 1)
        {
            if (_owner.herding.visibleTargets.Count > 1)
            {
                foreach (Transform item in _owner.herding.visibleTargets)
                {
                    if (item.gameObject.tag == "Rapty")
                        return;
                }
                _owner.aStar.target = _owner.herding.visibleTargets[0].gameObject;
                _owner.SM.ChangeState(HerdingState.Instance);
                return;
            }
        }

    }
}
