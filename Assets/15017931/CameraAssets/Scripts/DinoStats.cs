using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinoStats : MonoBehaviour {

	public Text dinoIdText;
	public Text healthText;
	public Text hungerText;
	public Text positionText;

	public void setDinoIDText (string id)
	{
		dinoIdText.text = id;
	}

	public void setHealthText(int newHealth)
	{
		healthText.text = newHealth.ToString();
	}

	public void setHungerText(int newHunger)
	{
		hungerText.text = newHunger.ToString();
	}

	public void setPosText(Vector3 newPosition)
	{
		positionText.text = newPosition.ToString();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
