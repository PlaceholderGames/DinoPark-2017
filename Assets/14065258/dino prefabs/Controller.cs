using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public List<GameObject> allRaptors = new List<GameObject>();
    public GameObject[] raptorArray;
    // Use this for initialization
    void Start () {
        if (allRaptors == null)
            raptorArray = GameObject.FindGameObjectsWithTag("Rapty");

        foreach (GameObject item in raptorArray)
        {
            allRaptors.Add(item);
        }
    }
	
	// Update is called once per frame
	void Update () {

    }
}
