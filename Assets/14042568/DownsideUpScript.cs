using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownsideUpScript : MonoBehaviour {



    // Use this for initialization
    void Start () {
		
	}

    public void DownsideUpMeBro(MyRapty x)
    {
        StartCoroutine(DownyUppyDoos(x));

    }	

    IEnumerator DownyUppyDoos(MyRapty x)
    {
        yield return new WaitForSeconds(1.0f);
        x.transform.Rotate(0.0f, 0.0f, 180.0f);

    }
	// Update is called once per frame
	void Update () {
		
	}
}
