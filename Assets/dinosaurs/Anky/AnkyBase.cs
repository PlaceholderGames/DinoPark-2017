using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkyBase : MonoBehaviour {
    public Animator myAnim;

    // Use this for initialization
    void Start () {
        myAnim = GetComponent<Animator>();

    }

	// Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("n"))
        {
            myAnim.Play("Start eating", -1, 0f);
        }
        if (Input.GetKeyUp("n"))
        {
            myAnim.Play("Stop eating", -1, 0f);
        }
        if (Input.GetKeyDown("m"))
        {
            myAnim.Play("Walk", -1, 0f);
        }
        if (Input.GetKeyUp("m"))
        {
            myAnim.Play("Idle", -1, 0f);
        }
        if (Input.GetKeyDown("b"))
        {
            myAnim.Play("Attack", -1, 0f);
        }
    }
}
