using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour {

	void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Anky")
        {
            Destroy(col.gameObject);
        }
    }
}
