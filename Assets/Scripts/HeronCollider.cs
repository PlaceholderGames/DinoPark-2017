using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronCollider : MonoBehaviour {

	public float radius = 0.00f;
	public Vector3 position;

	// Use this for initialization
	void Start () {
		position = transform.position;
	}

	void OnDrawGizmoSelected()
	{
		Gizmos.color = new Color (0.32f, 0.55f, 0.76f, 0.7f);
		Gizmos.DrawSphere (transform.position, radius);
	}
}
