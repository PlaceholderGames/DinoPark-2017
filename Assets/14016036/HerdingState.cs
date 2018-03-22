using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

public class HerdingState : State<MyAnky>
{
    private static HerdingState _instance;
    Transform closestTarget;
    Transform target;
    Transform targetCloseEnough;
    int targetIndexValue;
    private HerdingState()
    {
        if (_instance != null)
        {
            return;
        }


        _instance = this;
    }

    public static HerdingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new HerdingState();
            }
            return _instance;
        }
    }


    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Herding State");
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Herding State");
    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.ankyAgent.maxSpeed = 3;
        _owner.currentState = MyAnky.ankyState.HERDING;
        _owner.anim.SetBool("isHerding", true);
        _owner.anim.SetBool("isAlerted", false);
        _owner.anim.SetBool("isDrinking", false);
        _owner.anim.SetBool("isGrazing", false);
        _owner.anim.SetBool("isIdle", false);
        _owner.anim.SetBool("isEating", false);
        _owner.anim.SetBool("isAttacking", false);
        _owner.anim.SetBool("isFleeing", false);
        _owner.anim.SetBool("isDead", false);
        if (_owner.ankyFriendliesClose.Count != 0)
        {
            for (int i = 0; i < _owner.ankyFriendliesClose.Count; i++)
            {
                closestTarget = _owner.ankyFriendliesClose[i];
                float dist = Vector3.Distance(closestTarget.position, _owner.transform.position);
                Debug.Log("dist " + dist);
                if (_owner.closestDist > dist)
                {
                    _owner.closestDist = dist;
                    targetIndexValue = i;
                }
            }
            Debug.Log(targetIndexValue);
            _owner.ankySeek.target = _owner.ankyFriendliesClose[targetIndexValue].gameObject;
            //_owner.ankyFace.target = _owner.ankyFriendliesClose[targetIndexValue].gameObject;
            _owner.ankySeek.enabled = true;
            //_owner.ankyFace.enabled = true;
            _owner.ankyWander.enabled = false;
        }
        else if (_owner.ankyFriendliesFar.Count != 0)
        {
            for (int j = 0; j < _owner.ankyFriendliesFar.Count; j++)
            {
                closestTarget = _owner.ankyFriendliesFar[j];
                float dist = Vector3.Distance(target.position, _owner.transform.position);

                if (_owner.closestDist > dist)
                {
                    _owner.closestDist = dist;
                    targetIndexValue = j;
                }
            }
            _owner.ankySeek.target = _owner.ankyFriendliesFar[targetIndexValue].gameObject;
            //_owner.ankyFace.target = _owner.ankyFriendliesClose[targetIndexValue].gameObject;
            _owner.ankySeek.enabled = true;
            //_owner.ankyFace.enabled = true;
            _owner.ankyWander.enabled = false;
        }
        if (Vector3.Distance(_owner.ankySeek.target.transform.position, _owner.transform.position) < 16)
        {
            Debug.Log("am i getting in the change back to AlertState condition");
            if (_owner.anky == 1)
            {
                _owner.ankyAgent.maxSpeed = 1;
                _owner.stateMachine.ChangeState(WanderingState.Instance);
            }
            else
            {
                _owner.ankyAgent.maxSpeed = 1;
                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
            //_owner.closestDist = 9999;
        }

    }
}


//for (int i = 0; i < _owner.ankyFriendliesFar.Count; i++)
//{
//    Transform target = _owner.ankyFriendliesFar[i];
//    float dist = Vector3.Distance(target.position, _owner.transform.position);

//    if (_owner.closestDist > dist)
//    {
//        _owner.closestDist = dist;
//        closestTarget = target;
//    }
//}
////_owner.ankySeek.target = closestTarget.position;
//_owner.ankyWander.enabled = false;
//_owner.ankySeek.enabled = true;
//if (Vector3.Distance(closestTarget.transform.position, _owner.transform.position) < 5)
//{
//    _owner.stateMachine.ChangeState(AlertState.Instance);
//}



//for (int i = 0; i < _owner.ankyView.visibleTargets.Count; i++)
//{
//    Transform target = _owner.ankyView.visibleTargets[i];
//    if (_owner.ankyView.visibleTargets[i].tag == "Anky" && Vector3.Distance(target.position, _owner.transform.position) > 15)
//    {
//        if(_owner.ankyFriendliesClose.Count != 0)
//        {
//            Debug.Log("Getting into friendlyClose function");
//            for (int j = 0; j < _owner.ankyFriendliesClose.Count; j++)
//            {
//                closestTarget = _owner.ankyFriendliesClose[i];
//                float dist = Vector3.Distance(target.position, _owner.transform.position);
//
//                if (_owner.closestDist > dist)
//                {
//                    _owner.closestDist = dist;
//                    targetIndexValue = i;
//                }
//            }
//            _owner.ankySeek.target = _owner.ankyFriendliesClose[targetIndexValue].gameObject;
//            _owner.ankySeek.enabled = true;
//            _owner.ankyWander.enabled = false;                    
//        }
//        else if(_owner.ankyFriendliesFar.Count !=0)
//        {
//            Debug.Log("Getting into friendlyFar function");
//            for (int j = 0; j < _owner.ankyFriendliesFar.Count; j++)
//            {
//                //Debug.Log("J = " + j);
//                closestTarget = _owner.ankyFriendliesFar[j];
//                float dist = Vector3.Distance(target.position, _owner.transform.position);
//
//                if (_owner.closestDist > dist)
//                {
//                    _owner.closestDist = dist;
//                    targetIndexValue = j;
//                }
//            }
//            _owner.ankySeek.target = _owner.ankyFriendliesFar[targetIndexValue].gameObject;
//            _owner.ankySeek.enabled = true;
//            _owner.ankyWander.enabled = false;                 
//        }
//        if (Vector3.Distance(_owner.ankySeek.target.transform.position, _owner.transform.position) < 16)
//        {
//            Debug.Log("am i getting in the change back to AlertState condition");
//            _owner.stateMachine.ChangeState(AlertState.Instance);
//            _owner.ankyFriendliesClose.Clear();
//            _owner.ankyFriendliesFar.Clear();
//            //_owner.closestDist = 9999;
//        }
//    }
//
//}