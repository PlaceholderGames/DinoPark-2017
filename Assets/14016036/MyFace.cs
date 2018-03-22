using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFace : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetPoint;
    public Quaternion targetRotation;


    void Update()
    {
        targetPoint = new Vector3(target.transform.position.x, target.transform.transform.position.y, target.transform.position.z) - transform.position;
        targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    }
}

