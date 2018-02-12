using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryCamera : MonoBehaviour {

    Vector3 myPos;
    Vector3 offset;
    Vector3 centreScreen;
    public Transform objPosition;
    public Transform playerPosition;
    public Camera dinoCam;
    float x;
    float y;
    float speed;
    public Image dinoMenu;

    // Use this for initialization
    void Start () {
        dinoMenu.enabled = false;
        dinoCam.enabled = false;
        offset = new Vector3(0.0f, 5.0f, 0.0f);
        x = Screen.width / 2;
        y = Screen.height / 2;
        centreScreen = new Vector3(x, y);
        speed = 3.0f;
        dinoMenu.rectTransform.sizeDelta = new Vector2(Screen.width * 0.22f, Screen.height * 0.5f);       
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
        if (dinoCam.enabled == true)
            dinoMenu.enabled = true;
        else
            dinoMenu.enabled = false;
    }
}
