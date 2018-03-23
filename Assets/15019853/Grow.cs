using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour {
    Vector3 scale;
	// Use this for initialization
	void Start ()
    {
        scale = new Vector3(1, 1, 1);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (transform.localScale.x < scale.x || transform.localScale.y < scale.y || transform.localScale.z < scale.z)
        {
            transform.localScale += new Vector3(0.01f * Time.deltaTime, 0.01f * Time.deltaTime, 0.01f * Time.deltaTime);
        }	
	}
}
