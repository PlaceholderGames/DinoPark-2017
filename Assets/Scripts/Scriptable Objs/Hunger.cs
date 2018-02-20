using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Hunger")]
public class Hunger : ScriptableObject {

	float hunger;

    void Start ()
    {
        hunger = 100;
    }

    void setHunger(float H)
    {
        hunger = H;
    }
	
    float getHunger()
    {
        return hunger;
    }

	// Update is called once per frame
	void Update () {
        hunger -= 0.01f;
	}
}
