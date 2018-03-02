using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFPS : MonoBehaviour {

    //public GameObject fpsCamera;
    public GameObject freeCamPosition;
    public Transform moveFPS;
    public GameObject fpsCam;
    public GameObject freecam;
    public FPSFreeCam fpsFreeCamScript;


    void Start()
    {
        
    }
    void Update()
    {
        if (fpsCam.activeInHierarchy == true)
        {
            if (Input.GetButton("r"))
            {
                fpsFreeCamScript.movePosition();
            }
        }
    }
    // Use this for initialization
    public void movePosition()
    {
        
        moveFPS.transform.position = freeCamPosition.transform.position;
        freecam.SetActive(false);
        fpsCam.SetActive(true);
    }
}
