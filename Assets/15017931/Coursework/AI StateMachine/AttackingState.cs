using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

public class AttackingState : State<MyAnky>
{
    private static AttackingState _instance;
    private float attackCooldown = 2;
    private float counter = 2;
    private List<Transform> targetInRange;
    public GameObject pointToAim = null;

    private AttackingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AttackingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackingState();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Attacking State");
        _owner.anim.SetBool("isAttacking", true);
        _owner.anim.SetBool("isAlerted", true);
        _owner.setCurrentState(MyAnky.ankyState.ATTACKING);
        targetInRange = new List<Transform>();
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Attacking State");
        _owner.anim.SetBool("isAttacking", false);
    }

    public override void UpdateState(MyAnky _owner)
    {
        _owner.averageTargetPos = Vector3.zero;
        targetInRange = new List<Transform>();

        if (_owner.myStats.health <= 0)
        {
            _owner.stateMachine.ChangeState(DeadState.Instance);
        }
        else if (_owner.predatorsInRange.Count > 0)
        {
            foreach (Transform pred in _owner.predatorsInRange)
            {
                //Distance between us and predator
                _owner.distance = Vector3.Distance(_owner.transform.position, pred.transform.position);
                //find the closets predator
                if (_owner.distance < _owner.closestHazardDist)
                {
                    _owner.closestHazard = pred;
                    _owner.closestHazardDist = _owner.distance;
                }

                if (_owner.distance <= _owner.attackRange)
                {
                    Debug.Log("Predator in attack Range");
                    targetInRange.Add(pred);
                }
            }

            if (_owner.closestHazardDist > _owner.attackRange || _owner.myStats.health <= 30)
            {
                _owner.anim.SetBool("isFleeing", true);
                _owner.stateMachine.ChangeState(FleeState.Instance);
            }
            //check if the state of our target is dead or no targets within range
            else if (_owner.predatorsInRange.Count <=0)
            {
                _owner.stateMachine.ChangeState(AlertState.Instance);
            }
            else
            {
                //What to do if we are still attacking
                //Turn to face our main target

                foreach (Transform target in targetInRange)
                {
                    _owner.averageTargetPos += target.position;
                }

                _owner.averageTargetPos = new Vector3(_owner.averageTargetPos.x / targetInRange.Count, _owner.averageTargetPos.y / targetInRange.Count, _owner.averageTargetPos.z / targetInRange.Count);


                //every 2 seconds, swing our tail/ attack
                if (counter <= 0)
                {
                    Debug.Log("Swing");
                    //reset our cooldown
                    counter = attackCooldown;
                }
                counter -= Time.deltaTime;
            }
        }
    }
}
