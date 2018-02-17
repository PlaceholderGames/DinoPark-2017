using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_DinoStats : MonoBehaviour {

	public string dinoTypeTag;
	//GUI Stat Panels
	public  GameObject[] HUDStatPanels;

	//List of dinos in the world
	GameObject[] dinoList;

	// Use this for initialization
	void Start () {
		//get all dinos with specified tag
		dinoList = GameObject.FindGameObjectsWithTag (dinoTypeTag);
		//get all gameobjects with the tag dinostats

		HUDStatPanels = GameObject.FindGameObjectsWithTag("DinoStats");
	
		//HUDStatPanels = GameObject.Find("/DinoStats");


	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject x in dinoList) {
			//Debug.Log ( x.name + " Location: " + x.transform.position);
		}

		//Only populate panels if dinos exist
		for (int i = 0; i < dinoList.Length; i++) {
			HUDStatPanels [i].GetComponent<DinoStats> ().setPosText (dinoList [i].transform.position);
		}
	}
}
