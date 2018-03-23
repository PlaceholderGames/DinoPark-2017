using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;
using System;

public class Grazing : State<MyAnky>
{
    private static Grazing _Instance;
    private GameObject target;


    private Grazing()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static Grazing Instance
    {
        get
        {
            if (_Instance == null)
            {
                new Grazing();
            }
            return _Instance;
        }
    }


    public override void EnterState(MyAnky _Owner)
    {

        Debug.Log("entering Grazing state");
        

    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exiting Grazing state");
        _Owner.ankyWandering.enabled = false;
        _Owner.ankyAStar.enabled = false;
        _Owner.ankyASIntance.enabled = false;
        _Owner.ankyAsPath.enabled = false;
    }

    public override void UpdateState(MyAnky _Owner)
    {
       
        _Owner.ankyWandering.enabled = true;    // wandering gets enabled 

        if (_Owner.AnkyFriend != null && _Owner.ankyDistance > 70)  // if anky is visable and is over 70 away
        {
            _Owner.stateMachine.ChangeState(HerdingState.Instance); // switch to herding state
        }

        
        else if (_Owner.RaptyEnemy != null && _Owner.raptorDistance < 35)   // if enemy visible and  under 35 away go to alerted
        {
            Debug.Log("enemy has been seen");
            _Owner.stateMachine.ChangeState(Alert.Instance);    // switch to alert state
        }

        else if (_Owner.RaptyEnemy == null)   // if no enemys near and not herding start to wander
        {
            _Owner.ankyWandering.enabled = true;

            if (_Owner.thirst < 500 && _Owner.hunger > 500)   // if thirst under 500 and hunger over 500 find water
            {
                GetWater(_Owner); 
            }
            else if (_Owner.hunger < 500 && _Owner.thirst > 500)  // or if hunger under 50 and water over find food
            {
                GetFood(_Owner);
            }
            else if (_Owner.hunger < 500 && _Owner.thirst < 500)  // if both food and water are under 50 then find food
            {
                GetFood(_Owner);
            }
        }
        else if (_Owner.hunger <= 0 && _Owner.thirst <= 0)  // if hunger and thirst are less than 0
        {
            _Owner.stateMachine.ChangeState(DeadState.Instance);    // switch to dead state
        }
    }




    private void GetFood(MyAnky _Owner)
    {
        float ClosestFood = 9999;
        Transform FoodTarget = null;
        float distanceToFood = 9999;

        _Owner.ankyWandering.enabled = false;

        for (int i = 0; i < _Owner.FoodObjects.Count; i++)
        {
            distanceToFood = Vector3.Distance(_Owner.myTransform.position, _Owner.FoodObjects[i].transform.position);
            Debug.Log("Need a Food");
            //Debug.Log(distance);
            Vector3.Distance(_Owner.transform.position, _Owner.FoodObjects[i].transform.position);            
            if (distanceToFood < ClosestFood)
            {                
                ClosestFood = distanceToFood;
                FoodTarget = _Owner.FoodObjects[i];
            }
        }
        if (FoodTarget != null)
        {
            _Owner.ankyAStar.target = FoodTarget.gameObject;
            _Owner.ankyAStar.enabled = true;
            _Owner.ankyASIntance.enabled = true;
            _Owner.ankyAsPath.enabled = true;

            if (_Owner.myTransform.position.y > 60)
            {
                _Owner.ankyAStar.enabled = false;
                _Owner.ankyASIntance.enabled = false;
                _Owner.ankyAsPath.enabled = false;
                _Owner.stateMachine.ChangeState(Eating.Instance);
            }
        }
        else
        {
            _Owner.ankyAStar.enabled = false;
        }
    }



    private void GetWater(MyAnky _Owner)
    {
        _Owner.ankyWandering.enabled = false;
        float Closest = 9999;
        Transform WaterTarget = null;
        float distance = 9999;

        for (int i = 0; i < _Owner.WaterObjects.Count; i++)
        {
            distance = Vector3.Distance(_Owner.myTransform.position, _Owner.WaterObjects[i].transform.position);
            Debug.Log("Need a Drink");
            Vector3.Distance(_Owner.transform.position, _Owner.WaterObjects[i].transform.position);

            if (distance < Closest)
            {
                Closest = distance;
                WaterTarget = _Owner.WaterObjects[i];
            }
        }

        if (WaterTarget != null)
        {
            _Owner.ankyAStar.target = WaterTarget.gameObject;

            _Owner.ankyAStar.enabled = true;
            _Owner.ankyASIntance.enabled = true;
            _Owner.ankyAsPath.enabled = true;

            if (_Owner.myTransform.position.y < 40)
            {
                _Owner.ankyAStar.enabled = false;
                _Owner.ankyASIntance.enabled = false;
                _Owner.ankyAsPath.enabled = false;

                _Owner.stateMachine.ChangeState(Drinking.Instance);
            }
        }
        else
        {
            _Owner.ankyAStar.enabled = false;
        }
    }


}
