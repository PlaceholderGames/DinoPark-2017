using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronCollider : MonoBehaviour {

    float radius = 0.00f;
    Vector3 position;

    void Awake()
    {
        position = transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color(0.32, 0.55, 0.76, 0.7);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
