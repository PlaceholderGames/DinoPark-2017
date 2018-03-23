using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckAnky : MonoBehaviour {

    private AnkyAI anky;

    private void Start()
    {
        anky = GetComponent<AnkyAI>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Rapty" && anky.health > 0)
        {
            anky.health -= 5;

        }
    }
}
