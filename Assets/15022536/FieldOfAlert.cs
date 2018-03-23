﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfAlert : MonoBehaviour
{

    public float viewRadius; // Range of vision. Anky = 100, Rapty = 200
    [Range(0, 360)]
    public float viewAngle; // Field of Vision. Anky = 300, Rapty = 240
    [Range(0, 180)]
    public float stereoAngle; 

    public LayerMask targetMask;
    //public LayerMask obstacleMask; // Not using raytracing to determine visibility right now

    [HideInInspector] // If you want to see the list of visible Dinos in the Inspector view, comment this out
    public List<Transform> visibleTargets = new List<Transform>(); // this is the list of visible dinosaurs

    [HideInInspector] // If you want to see the list of visible Dinos in the Inspector view, comment this out
    public List<Transform> stereoVisibleTargets = new List<Transform>(); // this is the list of visible dinosaurs in stereo

    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < stereoAngle / 2) // divide by two so it can be seen in stereo
            {
               
                stereoVisibleTargets.Add(target);
            }
            else if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                
                visibleTargets.Add(target); 
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
