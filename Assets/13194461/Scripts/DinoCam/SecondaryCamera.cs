using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryCamera : MonoBehaviour {

    Vector3 myPos;
    Vector3 offset;
    Vector3 centreScreen;
    public Transform objPosition;
    public Camera dinoCam;
    float x;
    float y;
    float speed;
    GameObject canvas;

    // Use this for initialization
    void Start () {
        dinoCam.enabled = false;
        offset = new Vector3(0.0f, 5.0f, 0.0f);
        x = Screen.width / 2;
        y = Screen.height / 2;
        centreScreen = new Vector3(x, y);
        speed = 3.0f;

        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit objHit;
            Ray ray = Camera.main.ScreenPointToRay(centreScreen);

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

        transform.position = objPosition.position - (objPosition.forward * 5) + offset;
        transform.rotation = Quaternion.Lerp(transform.rotation, objPosition.transform.rotation, Time.deltaTime * speed);

        if (dinoCam.enabled)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }
}
