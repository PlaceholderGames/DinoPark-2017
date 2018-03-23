using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateStuff;

public class DrinkingState : State<MyRapty>
{
    public float waterDistance;
    public bool foundWater = false;
    private GameObject waterTemp = new GameObject();
    private static DrinkingState _Instance;
    private DrinkingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static DrinkingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new DrinkingState();
            }

            return _Instance;
        }
    }



    public override void enterState(MyRapty _owner)
    {
        Debug.Log("Entering Drinking State");
      
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Drinkning State");
    }

    public override void UpdateState(MyRapty _owner)
    {

        if (!foundWater)
        {
            waterSearch(_owner, _owner.waterList);
        }
       

        float tempDistance = Vector3.Distance(_owner.closestWater.transform.position, _owner.transform.position);

        if (tempDistance < 15 )
        {
            _owner.StartCoroutine(Drink(_owner));
            
        }
        //////fill water
       
       
         
        if (_owner.myFOV.visibleTargets.Count > 0 && _owner.thirstLevel > 75)
        {
            _owner.myA_star.enabled = false;
            _owner.myAS_instance.enabled = false;
            _owner.myAS_pathFollower.enabled = false;
            _owner.raptyMachine.ChangeState(HuntingState.Instance);
        }
        
        if (_owner.thirstLevel == 100)
        {
            _owner.myA_star.enabled = false;
            _owner.myAS_instance.enabled = false;
            _owner.myAS_pathFollower.enabled = false;
            _owner.raptyMachine.ChangeState(SearchingState.Instance);
        }

    }
    IEnumerator Drink(MyRapty _owner)
    {
        yield return new WaitForSeconds(1.0f);
        foundWater = true;
        _owner.thirstLevel += 5;

        if (_owner.thirstLevel > 100)
        {
            _owner.thirstLevel = 100;
            foundWater = false;
        }



    }

    void waterSearch(MyRapty _owner, List<Transform> x)
    {
        float dfgh = 320000.0f;
        // availableDinos = GameObject.FindGameObjectsWithTag("Anky");  

   
        for (int i = 0; i < x.Count; i++)
        {

            waterDistance = Vector3.Distance(x[i].transform.position, _owner.transform.position);

            if (waterDistance < dfgh)
            {

                dfgh = waterDistance;
                _owner.dinoCheck = i;
                waterTemp = x[i].gameObject;

            }
        }
       
        _owner.closestWater = new GameObject();
        _owner.closestWater = waterTemp;
        _owner.myPursue.enabled = false;
        _owner.myA_star.target = _owner.closestWater;
        _owner.myA_star.enabled = true;
        _owner.myAS_instance.enabled = true;
        _owner.myAS_pathFollower.enabled = true;
    }
}