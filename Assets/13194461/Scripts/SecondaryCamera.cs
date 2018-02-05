using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCamera : MonoBehaviour {

    Vector3 myPos;
    Vector3 offset;
    public Transform objPosition;
    public Camera dinoCam;

	// Use this for initialization
	void Start () {
        dinoCam.enabled = false;
        offset = new Vector3(0.0f, 5.0f, -5.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit objHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out objHit))
            {
                if (objHit.transform.name == "HeronPrefab")
                {
                    objPosition = objHit.transform;
                    dinoCam.enabled = true;
                }
                else
                {
                    dinoCam.enabled = false;
                }
                  
                  
            }
            else
            {
                dinoCam.enabled = false;
            }
        }

        transform.position = objPosition.position + myPos + offset;
	}
}
