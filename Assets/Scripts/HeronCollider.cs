using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronCollider : MonoBehaviour {

	private double radius = 0.00f;
	private Vector3 position;

	// Use this for initialization
	void Start () {
		position = transform.position;
	}

	void OnDrawGizmoSelected()
	{
		Gizmos.color = new Color (0.32, 0.55, 0.76, 0.7);
		Gizmos.DrawWireSphere (transform.position, radius);
	}
}
