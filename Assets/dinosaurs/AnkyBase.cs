using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkyBase : MonoBehaviour {
    public Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

    }

	// Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("n"))
        {
            anim.Play("Idle", -1, 0f);
        }
	}
}
