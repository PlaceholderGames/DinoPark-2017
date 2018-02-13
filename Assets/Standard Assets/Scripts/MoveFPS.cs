using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFPS : MonoBehaviour {

    public GameObject fpsCamera;
    public GameObject freeCam;
    public Transform moveFPS;
    Camera fpsCam;
    public FPSFreeCam fpsFreeCam;
    public Transform mainCamera;


    void Start()
    {
        
    }
    void Update()
    {
        if (fpsCamera.activeSelf == true)
        {
            if (Input.GetButton("r"))
            {
                freeCam.SetActive(true);
                mainCamera.parent = freeCam.transform;
                fpsFreeCam.movePosition();
                fpsCamera.SetActive(false);
            }
        }
    }
    // Use this for initialization
    public void movePosition()
    {
        mainCamera.transform.position = freeCam.transform.position;
        moveFPS.transform.position = freeCam.transform.position;
    }
}
