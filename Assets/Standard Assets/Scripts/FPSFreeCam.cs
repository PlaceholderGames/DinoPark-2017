using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSFreeCam : MonoBehaviour {
    // Use this for initialization
    public float speed = 20.0f;
    public float maxVelocityChange = 10.0f;
    //public Vector3 speed = new Vector3(3, 3, 3);
    //public GameObject cameraTransform;
    public GameObject freeCam;
    public GameObject firstPersonCamera;
    public Transform freecam;
    public MoveFPS movefpsScript;
    public GameObject fpsCam;
    public GameObject FreeCamera;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (FreeCamera.activeInHierarchy == true)
        {
            if (Input.GetButton("f"))
            {
                movefpsScript.movePosition();
                
            }
        }
    }
    public void movePosition()
    {
        
        freecam.transform.position = firstPersonCamera.transform.position;
        FreeCamera.SetActive(true);
        fpsCam.SetActive(false);
    }
    // Update is called once per frame
    void FixedUpdate()
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
    }
}
