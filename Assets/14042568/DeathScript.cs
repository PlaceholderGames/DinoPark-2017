using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour {

    private GameObject target;
    private Agent targetAgent;
    public Wander myWander;
    public Transform myTransform;
    public bool isDead = false;
    void Start () {
        targetAgent = GetComponent<Agent>();
        myWander = GetComponent<Wander>();
        myTransform = GetComponent<Transform>();
        
	}
    public void Die()
    {
        myWander.enabled = false;
        targetAgent.enabled = false;
        myTransform.Rotate(0.0f, 0.0f, 180.0f);
        isDead = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
