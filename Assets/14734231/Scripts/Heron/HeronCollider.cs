using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronCollider : MonoBehaviour {

    public float radius = 0.0f;
    public Vector3 position;

    private void Awake()
    {
        position = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.32f, 0.55f, 0.76f, 0.7f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
