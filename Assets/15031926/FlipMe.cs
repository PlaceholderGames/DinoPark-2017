using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipMe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Flip()
    {
        StartCoroutine(Flippo());
    }

    IEnumerator Flippo()
    {
        yield return new WaitForSeconds(1.0f);
        transform.Rotate(0.0f, 0.0f, 180.0f);
    }
}
