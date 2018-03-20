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
            anim.Play("Start eating", -1, 0f);
        }
        if (Input.GetKeyUp("n"))
        {
            anim.Play("Stop eating", -1, 0f);
        }
        if (Input.GetKeyDown("m"))
        {
            anim.Play("Walk", -1, 0f);
        }
        if (Input.GetKeyUp("m"))
        {
            anim.Play("Idle", -1, 0f);
        }
        if (Input.GetKeyDown("b"))
        {
            anim.Play("Attack", -1, 0f);
        }
    }
}
