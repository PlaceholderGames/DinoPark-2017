using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateStuff;


public class HuntingState : State<MyRapty>
{

    bool pursueBool = false;
    private static HuntingState _Instance;
    private HuntingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static HuntingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new HuntingState();
            }

            return _Instance;
        }
    }



    public override void enterState(MyRapty _owner)
    {
        aStarHunt(_owner);
        Debug.Log("Entering Hunting State");
        
        //_owner.SetSteering(_owner.myPursue.GetSteering());
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting First State");
    }

    public override void UpdateState(MyRapty _owner)
    {
        float tempDistance = Vector3.Distance(_owner.closestAnky.transform.position, _owner.transform.position);
        Debug.Log(tempDistance);

        if (tempDistance > 30)
        {
            Debug.Log("a star if");
            if (pursueBool)
            {
                aStarHunt(_owner);
            }       
        }
        else
        {
            Debug.Log("pursue  if");
            if (!pursueBool)
            {
                pursueHunt(_owner);
            }
            AttackCheck(_owner, tempDistance);
        }

        //if (_owner.transform.position == _owner.myA_star.target.transform.position)
        //{
        //    pursueHunt(_owner);
        //}

       
       DrinkCheck(_owner);
       // FleeCheck(_owner);
    }


    void aStarHunt(MyRapty _owner)
    {
        pursueBool = false;
        _owner.myPursue.enabled = false;
        _owner.myA_star.target = _owner.closestAnky;
        _owner.myA_star.enabled = true;
        _owner.myAS_instance.enabled = true;
        _owner.myAS_pathFollower.enabled = true;
    }

    void pursueHunt(MyRapty _owner)
    {
        pursueBool = true;
        _owner.myA_star.enabled = false;
        _owner.myAS_instance.enabled = false;
        _owner.myAS_pathFollower.enabled = false;
        _owner.myPursue.target = _owner.closestAnky;
        _owner.myPursue.newTarget();
        _owner.maxSpeed = 4;
        _owner.myPursue.enabled = true;
    }

    void AttackCheck(MyRapty _owner, float x)
    {
        

        if (x <  5)
        {
            _owner.raptyMachine.ChangeState(AttackingState.Instance);

        }
        
    }

    void DrinkCheck(MyRapty _owner)
    {
        float raptyY = _owner.transform.position.y;

        if (raptyY < 75)
        {
            if (_owner.thirstLevel < 20)
            {
                //move down to water level here
                //
                _owner.raptyMachine.ChangeState(DrinkingState.Instance);

            }

        }
    }

    void FleeCheck(MyRapty _owner)
    {
        if (_owner.health < 10 && _owner.hunger > 50)
        {
            _owner.raptyMachine.ChangeState(FleeingState.Instance);
        }
    }
}
