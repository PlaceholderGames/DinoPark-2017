using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour {

    // Use this for initialization
    public float speed = 0.4F;
    public Transform cameraTransform;
    CursorLockMode wantedMode;
    void Start () {
        

        // Apply requested cursor state
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("w"))
        {

            transform.Translate(Vector3.forward * speed / 6);
            //transform.Translate(Vector3.forward * speed / 6);
        }
        if (Input.GetButton("a"))
        {
            transform.Translate(Vector3.left * speed / 6);
        }
        if (Input.GetButton("s"))
        {
            transform.Translate(Vector3.back * speed / 6);
        }
        if (Input.GetButton("d"))
        {
            transform.Translate(Vector3.right * speed / 6);
        }
        if (Input.GetButton("e"))
        {
            transform.Translate(Vector3.up * speed / 6);
        }
        if (Input.GetButton("q"))
        {
            transform.Translate(Vector3.down * speed / 6);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain")
        {
            Debug.Log("I'm hitting the terrain collidor");
        }
    }
}
