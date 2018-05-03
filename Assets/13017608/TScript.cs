using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TScript : MonoBehaviour {

    public Terrain Terrain;
	// Use this for initialization
	void Start () {
        Terrain.GetComponent<Terrain>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
