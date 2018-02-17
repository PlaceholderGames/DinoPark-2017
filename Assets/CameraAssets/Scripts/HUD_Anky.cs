using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Anky : MonoBehaviour {

	public string dinoTypeTag;

	private GameObject[] dinoList;

	// Use this for initialization
	void Start () {
		dinoList = GameObject.FindGameObjectsWithTag (dinoTypeTag);
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject x in dinoList) {
			Debug.Log ( x.name + " Location: " + x.transform.position);
		}
	}
}
