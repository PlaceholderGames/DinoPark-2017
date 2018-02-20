using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graze : MonoBehaviour {

    public Hunger hungerSO;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        float distance = 100f;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            Renderer rend = hit.transform.GetComponent<Renderer>();
            Texture2D tex = rend.material.mainTexture as Texture2D;

            // if tex == grass then eat
        }
    }
}
