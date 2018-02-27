using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldOfView : MonoBehaviour {
    
    // the total view, restricted to 0-360
    public float viewRadius;
    [Range(0, 360)]
    //the view we want to see
    public float viewAngle;
    public float damage;

    //the layers for everything we cant see through and what we're looking for
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    //a list that needs to be sent to another script. but shouldnt be edited by anyone.
    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    //Starts working straight away. this could be changed later or not. 
    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", 1.0f);
    }

    //happens every .2 of a tick, can be adjusted to help with framerate if adding more dino's might cause
    //it to slow down
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    
    //main function to find what we're looking for
    void FindVisibleTargets()
    {
        //clears the list so we dont end up with duplicates
        visibleTargets.Clear();
        //list to hold everything that's in our view.
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //for everything in the view
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            //get its postion
            Transform target = targetsInViewRadius[i].transform;
            //how far away it is from us
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            //if its inside of our view angle
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                //target.SendMessage("inArea", transform.position);
                //get the distance between us and the target
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                //make sure its not blocked by the any obstacles between our two objects
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) 
                {
                    //save it. This is where you can send messages to the target to say 
                    //"ive been hit. take 10 damage or such"
                    visibleTargets.Add(target);
                    target.SendMessage("dealDamage", damage);
                } 
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
