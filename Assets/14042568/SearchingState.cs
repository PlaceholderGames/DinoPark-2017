using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateStuff;

public class SearchingState : State<MyRapty>
{

    float tempDistance;
    public List<Transform> tempList;
    private static SearchingState _Instance;
    private GameObject Dino = new GameObject();
    private SearchingState()
   
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static SearchingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new SearchingState();
            }

            return _Instance;
        }
    }



    public override void enterState(MyRapty _owner)
    {
        // sightTest(_owner);
        Debug.Log("Entering Searching State");
        
        // GameObject[] availableDinos;


    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("Exiting Searching State");
        
    }

    public override void UpdateState(MyRapty _owner)
    {
        Debug.Log("I'm in the update state" + _owner.waterList.Count);
        _owner.myWander.newWander();
        _owner.myWander.enabled = true;
        thirstCheck(_owner);
        sightTest(_owner);

    }

    void sightTest(MyRapty _owner)
    {
            tempList = new List<Transform>();
            for (int i = 0; i < _owner.myFOV.visibleTargets.Count; i++)
            {
            if (_owner.myFOV.visibleTargets[i].gameObject.tag == "Anky" && (!_owner.myFOV.visibleTargets[i].gameObject.GetComponent<DeathScript>().isDead))
                {
                   tempList.Add(_owner.myFOV.visibleTargets[i]);
                    //Debug.Log(tempList[i].gameObject.tag);
                }
        }
           for (int i = 0; i < _owner.myFOV.stereoVisibleTargets.Count; i++)
            {
                if (_owner.myFOV.stereoVisibleTargets[i].gameObject.tag == "Anky" && (!_owner.myFOV.stereoVisibleTargets[i].gameObject.GetComponent<DeathScript>().isDead))
                {
                    tempList.Add(_owner.myFOV.stereoVisibleTargets[i]);
                    Debug.Log(tempList[i].gameObject.tag);
                }          
        }          
        if (tempList.Count != 0)
        {
            Searching(_owner, tempList);
        }
       
            //_owner.raptyMachine.ChangeState(sHuntingState.Instance);
        else
        {
            Debug.Log("Nothing in list");
          
            Debug.Log(_owner.myWander.enabled);
        }
            
        
      
    }

    void Searching(MyRapty _owner, List<Transform> x)
    {
        float dfgh = 320000.0f;
        Debug.Log("HEllo theeererere" + x.Count);
        for (int i = 0; i < x.Count; i++)
            {
                Debug.Log("HEllo theeererere");
                tempDistance = Vector3.Distance(x[i].transform.position, _owner.transform.position);
            if (tempDistance < dfgh)
                {
                    dfgh = tempDistance;
                    _owner.dinoCheck = i;
                    Dino = x[i].gameObject;
                }
            }
        _owner.myWander.enabled = false;
        _owner.closestAnky = new GameObject();
        _owner.closestAnky = Dino;
        _owner.raptyMachine.ChangeState(HuntingState.Instance);
    }

    void thirstCheck(MyRapty _owner)
    {

        if (_owner.thirstLevel < 20)
        {
            _owner.raptyMachine.ChangeState(DrinkingState.Instance);
        }
    }
 }