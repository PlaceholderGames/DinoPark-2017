using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSFreeCam : MonoBehaviour {

    // Use this for initialization
    public float speed = 20.0f;
    public float maxVelocityChange = 10.0f;
    //public Vector3 speed = new Vector3(3, 3, 3);
    public GameObject cameraTransform;
    Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
	}
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        //if (Input.GetButton("w"))
        //{
        //    Vector3 forward = new Vector3(0, 0, 1);
        //    //transform.Translate(Vector3.forward * speed / 6);
        //    rb.MovePosition(rb.position + forward * Time.deltaTime);
        //}
        //if (Input.GetButton("s"))
        //{
        //    Vector3 back = new Vector3(0, 0, -1);
        //    //transform.Translate(Vector3.back * speed / 6);
        //    rb.MovePosition(rb.position + back * Time.deltaTime);
        //}
        //if (Input.GetButton("a"))
        //{
        //    Vector3 left = new Vector3(-1, 0, 0);
        //    //transform.Translate(Vector3.left * speed / 6);
        //    rb.MovePosition(rb.position + left * Time.deltaTime);
        //}
        //if (Input.GetButton("d"))
        //{
        //    Vector3 right = new Vector3(1, 0, 0);
        //    //transform.Translate(Vector3.right * speed / 6);
        //    rb.MovePosition(rb.position + right * Time.deltaTime);
        //}
        //if (Input.GetButton("q"))
        //{
        //    Vector3 down = new Vector3(0, -1, 0);
        //    //transform.Translate(Vector3.down * speed / 100);
        //    rb.MovePosition(rb.position + down * Time.deltaTime);
        //}
        //if (Input.GetButton("e"))
        //{
        //    Vector3 up = new Vector3(0, 1, 0);
        //    //transform.Translate(Vector3.up * speed / 100);
        //    rb.MovePosition(rb.position + up * Time.deltaTime);
        //}
    }
}
