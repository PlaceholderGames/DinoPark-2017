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
            float[] tex = TerrainSurface.GetTextureMix(hit.collider.transform.position);
            Debug.Log("(" + tex[1] + ", " + tex[2] + ", " + tex[3] + ")");
            // if tex == grass then eat

            //(0, 0.7411765, 0.2588235)
        }
    }
}
