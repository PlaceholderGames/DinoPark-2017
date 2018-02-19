using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryCamera : MonoBehaviour {

    Vector3 myPos;
    Vector3 offset;
    Vector3 minimapOffset;
    Vector3 centreScreen;
    public Transform objPosition;
    public Transform playerPosition;
    public Camera dinoCam;
    float x;
    float y;
    float speed;
    public Image dinoMenu;
    public Image minimapBackground;
    bool UseMinimap;
    float minimapXPos;
    float minimapYPos;
    float minimapZPos;
    bool followPlayer;

    // Use this for initialization
    void Start() {
        UseMinimap = true;
        followPlayer = true;
        playerPosition = objPosition;
        dinoMenu.enabled = false;
        dinoCam.enabled = true;
        offset = new Vector3(0.0f, 5.0f, 0.0f);
        minimapOffset = new Vector3(0.0f, 25.0f, 0.0f);
        x = Screen.width / 2;
        y = Screen.height / 2;
        centreScreen = new Vector3(x, y);
        speed = 3.0f;
        dinoMenu.rectTransform.sizeDelta = new Vector2(Screen.width * 0.22f, Screen.height * 0.5f);
        minimapBackground.rectTransform.sizeDelta = new Vector2(Screen.width * 0.22f, Screen.height * 0.31f);
        transform.position = playerPosition.position + minimapOffset;
        transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
        minimapXPos = playerPosition.position.x;
        minimapYPos = playerPosition.position.y + minimapOffset.y;
        minimapZPos = playerPosition.position.z;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit objHit;
            Ray ray = Camera.main.ScreenPointToRay(centreScreen);

            if (Physics.Raycast(ray, out objHit))
            {
                if (objHit.transform.name == "HeronPrefab")
                {
                    objPosition = objHit.transform;
                    UseMinimap = false;
                }
                else
                {
                    UseMinimap = true;
                    transform.position = playerPosition.position + minimapOffset;
                    transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                    minimapXPos = playerPosition.position.x;
                    minimapYPos = playerPosition.position.y + minimapOffset.y;
                    minimapZPos = playerPosition.position.z;
                }
            }
            else
            {
                UseMinimap = true;
            }
        }

        if (UseMinimap == true)
            Minimap();
        else
            FollowCam();
    }

    void Minimap ()
    {
        dinoMenu.enabled = false;
        minimapBackground.enabled = true;

        if (Input.GetKey("[8]"))
        {
            minimapZPos += 0.1f;
            transform.position = new Vector3(minimapXPos, minimapYPos, minimapZPos);
        }

        if (Input.GetKey("[2]"))
        {
            minimapZPos -= 0.1f;
            transform.position = new Vector3(minimapXPos, minimapYPos, minimapZPos);
        }

        if (Input.GetKey("[4]"))
        {
            minimapXPos -= 0.1f;
            transform.position = new Vector3(minimapXPos, minimapYPos, minimapZPos);
        }

        if (Input.GetKey("[6]"))
        {
            minimapXPos += 0.1f;
            transform.position = new Vector3(minimapXPos, minimapYPos, minimapZPos);
        }

        if (Input.GetKey("[5]"))
        {
            followPlayer = !followPlayer;
            if (!followPlayer)
            {
                minimapXPos = playerPosition.position.x;
                minimapYPos = playerPosition.position.y + minimapOffset.y;
                minimapZPos = playerPosition.position.z;
            }
        }

        if (followPlayer)
            transform.position = playerPosition.position + minimapOffset;


    }

    void FollowCam()
    {
        transform.position = objPosition.position - (objPosition.forward * 5) + offset;
        transform.rotation = Quaternion.Lerp(transform.rotation, objPosition.transform.rotation, Time.deltaTime * speed);
        dinoMenu.enabled = true;
        minimapBackground.enabled = false;
    }
}
