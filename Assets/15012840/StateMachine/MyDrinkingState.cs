using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDrinkingState : MyState<MyAnky> {

    private static MyDrinkingState _instance;
    public MyDrinkingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static MyDrinkingState Instance
    {
        get
        {
            if (_instance == null)
            {
                return new MyDrinkingState();
            }
            return _instance;
        }
    }
    public override void EnterState(MyAnky _owner)
    {
        Debug.Log(_owner + " enters DRINKING");
    }
    public override void ExitState(MyAnky _owner)
    {
        Debug.Log(_owner + " exits DRINKING");
    }
    public override void UpdateState(MyAnky _owner)
    {

        _owner.wander.enabled = false;
        GameObject goTo = ClosestWater(_owner);
        if (goTo != null && _owner.transform.position.y > 35)
        {
            if (Vector3.Distance(goTo.transform.position, _owner.transform.position) > 40)
            {
                _owner.aS.target = goTo;
                _owner.flee.enabled = true;
            }
            else
            {
                Debug.Log("blah2");
                _owner.flee.enabled = false;
                _owner.seek.target = goTo;
                _owner.seek.enabled = true;
            }
        }
        if(_owner.transform.position.y <= 35)
        {
            Debug.Log("blah");
            _owner.seek.enabled = false;
            _owner.drinking.enabled = true;
            _owner.drinking.Drink();
        }

        //Alerted
        foreach (Transform animal in _owner.alerted.visibleTargets)
        {
            if (animal.tag == "Rapty")
            {
                _owner.mySM.ChangeState(MyAlertedState.Instance);
                return;
            }
        }
        //Idle
        if (_owner.health >= 100)
        {
            _owner.mySM.ChangeState(MyIdleState.Instance);
            return;
        }
        //Herding
        if (_owner.alerted.visibleTargets.Count > 1)
        {
            foreach (Transform item in _owner.alerted.visibleTargets)
            {
                if (item.gameObject.tag == "Rapty")
                    return;
            }
            foreach (var dino in _owner.alerted.visibleTargets)
            {
                Debug.Log(dino.position + " Dino");
                Debug.Log(_owner.transform.position + " Owner");
                if (dino.position != _owner.transform.position)
                {
                    _owner.aS.target = dino.gameObject;
                    _owner.mySM.ChangeState(MyHerdingState.Instance);
                    return;
                }
            }
        }
    }

    private GameObject ClosestWater(MyAnky _owner)
    {
        float mockDistance = 10000000.0f;
        GameObject closesWater = null;
        for (int i = 0; i < _owner.waterArray.Length; i++)
        {
            var distance = Vector3.Distance(_owner.transform.position, _owner.waterArray[i].gameObject.transform.position);
            if(distance < mockDistance)
            {
                mockDistance = distance;
                closesWater = _owner.waterArray[i].gameObject;
            }
        }
        return closesWater;
    }
}
