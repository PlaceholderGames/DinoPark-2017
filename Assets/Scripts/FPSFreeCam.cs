using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSFreeCam : MonoBehaviour {

    // Use this for initialization
    public float speed = 20.0f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("w"))
        {
            transform.Translate(Vector3.forward * speed / 6);
        }
        if (Input.GetButton("s"))
        {
            transform.Translate(Vector3.back * speed / 6);
        }
        if (Input.GetButton("a"))
        {
            transform.Translate(Vector3.left * speed / 6);
        }
        if (Input.GetButton("d"))
        {
            transform.Translate(Vector3.right * speed / 6);
        }
	}
}
