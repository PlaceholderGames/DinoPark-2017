using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapty : MonoBehaviour {
    public Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            anim.Play("Attack", -1, 0f);
        }
        if (Input.GetKeyDown("k"))
        {
            anim.Play("Alert", -1, 0f);
        }
        if (Input.GetKeyDown("l"))
        {
            anim.Play("Walk", -1, 0f);
        }
        if (Input.GetKeyUp("l"))
        {
            anim.Play("Idle", -1, 0f);
        }

    }
}
