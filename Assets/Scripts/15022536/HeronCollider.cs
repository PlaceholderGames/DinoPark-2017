using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronCollider : MonoBehaviour
{

    public double radius = 0.00;
    public Vector3 position;

    private void Awake()
    {
        position = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color(0.32, 0.55, 0.76, 0.7);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
