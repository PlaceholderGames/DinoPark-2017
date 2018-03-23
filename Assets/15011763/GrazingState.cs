using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
using System;

public class GrazingState : State<MyAnky>
{
    private static GrazingState _Instance;

    private GrazingState()
    {
        if (_Instance != null)
        {
            return;
        }
        _Instance = this;
    }

    public static GrazingState Instance
    {
        get
        {
            if (_Instance == null)
            {
                new GrazingState();
            }
            return _Instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {

        Debug.Log("enter GrazingState state");
        _Owner.myAnkyWander.enabled = true;
    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("exit GrazingState state");
        _Owner.myAnkyWander.enabled = false;
    }

    public override void UpdateState(MyAnky _Owner)
    {
        
        // If there are team dinos around, check the distances and call herding if it's at a certain distance
        if (_Owner.teamDino != null && _Owner.teamDistance > 50)
        {
            _Owner.myStateMachine.ChangeState(HerdingState.Instance);
        }

        // If there are enemy dinos around, turn off pathfinding if it's enabled
        // Go to Alerted
        if (_Owner.enemyDino != null)   
        {
            _Owner.myAnkyAStar.enabled = false;
            _Owner.myAnkyAStarInstance.enabled = false;
            _Owner.myAnkyPathFollower.enabled = false;
            _Owner.myStateMachine.ChangeState(AlertedState.Instance);
        }

        // If there are no enemy dinos around, handle checking for food and water
        if (_Owner.enemyDino == null)  
        {
            _Owner.myAnkyWander.enabled = true;

            if (_Owner.thirstyDino < 60 && _Owner.hungryDino > 60)
            {
                GoForWater(_Owner);
            }
            else if (_Owner.hungryDino < 60 && _Owner.thirstyDino > 60)
            {
                GoForFood(_Owner);
            }
            else if (_Owner.hungryDino < 60 && _Owner.thirstyDino < 60)
            {
                GoForFood(_Owner);
            }
        }

        // Kill dino if health is 0
        if (_Owner.healthyDino <= 0)
        {
            _Owner.myStateMachine.ChangeState(DeadState.Instance);
        }
    }

    private void GoForFood(MyAnky _Owner)
    {
        // Variables for checking for the closest food to path to
        float fFood = 10000;
        Transform fTarget = null;
        float fDistance = 10000;

        // Turn off Wandering
        _Owner.myAnkyWander.enabled = false;

        // Check the hungry list for the food objects
        // Check the distances of the objects
        for (int i = 0; i < _Owner.hungryList.Count; i++)
        {
            fDistance = Vector3.Distance(_Owner.myTransform.position, _Owner.hungryList[i].transform.position);
            if (fDistance < fFood)
            {
                fFood = fDistance;
                fTarget = _Owner.hungryList[i];
            }
        }

        // If there is a food object
        // Set the A Star Target and enable to scripts
        // When the Dino Reaches a certain height, stop the A Star and allow the dino to eat
        if (fTarget != null)
        {
            _Owner.myAnkyAStar.target = fTarget.gameObject;
            _Owner.myAnkyAStar.enabled = true;
            _Owner.myAnkyAStarInstance.enabled = true;
            _Owner.myAnkyPathFollower.enabled = true;

            if (_Owner.myTransform.position.y > 55)
            {
                _Owner.myAnkyAStar.enabled = false;
                _Owner.myAnkyAStarInstance.enabled = false;
                _Owner.myAnkyPathFollower.enabled = false;
                _Owner.myStateMachine.ChangeState(EatingState.Instance);
            }
        }
        else
        {
            _Owner.myAnkyAStar.enabled = false;
        }
    }

    private void GoForWater(MyAnky _Owner)
    {
        // Turn off Wandering
        _Owner.myAnkyWander.enabled = false;

        // 
        float wWater = 10000;
        Transform wTarget = null;
        float wDistance = 10000;

        // Check the thirsty list for the water objects
        // Check the distances of the objects
        for (int i = 0; i < _Owner.thirstyList.Count; i++)
        {
            wDistance = Vector3.Distance(_Owner.myTransform.position, _Owner.thirstyList[i].transform.position);
            if (wDistance < wWater)
            {
                wWater = wDistance;
                wTarget = _Owner.thirstyList[i];
            }
        }

        // If there is a water object
        // Set the A Star Target and enable to scripts
        // When the Dino Reaches a certain height, stop the A Star and allow the dino to eat
        if (wTarget != null)
        {
            _Owner.myAnkyAStar.target = wTarget.gameObject;
            _Owner.myAnkyAStar.enabled = true;
            _Owner.myAnkyAStarInstance.enabled = true;
            _Owner.myAnkyPathFollower.enabled = true;

            if (_Owner.myTransform.position.y < 39)
            {
                _Owner.myAnkyAStar.enabled = false;
                _Owner.myAnkyAStarInstance.enabled = false;
                _Owner.myAnkyPathFollower.enabled = false;
                _Owner.myStateMachine.ChangeState(DrinkingState.Instance);
            }
        }
        else
        {
            _Owner.myAnkyAStar.enabled = false;
        }
    }
}






