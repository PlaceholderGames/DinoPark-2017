using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour {

    Vector3 Scale;

	// Use this for initialization
	void Start () {
        Scale = new Vector3(1, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.localScale.x < Scale.x || transform.localScale.y < Scale.y || transform.localScale.z < Scale.z)
        {
            transform.localScale += new Vector3(0.01f * Time.deltaTime, 0.01f * Time.deltaTime, 0.01f * Time.deltaTime);
        }
	}
}
