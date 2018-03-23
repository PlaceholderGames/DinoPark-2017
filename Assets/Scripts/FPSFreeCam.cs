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
    public Transform mainCamera;
    MoveFPS movefps;
    
    Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
        
	}

    void Update()
    {
        if (freeCam.activeSelf == true)
        {
            if (Input.GetButton("f"))
            {
                firstPersonCamera.SetActive(true);
                mainCamera.parent = transform;
            }
        }
    }
    public void movePosition(float x, float y, float z)
    {

        freecam.transform.position = firstPersonCamera.transform.position;
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
