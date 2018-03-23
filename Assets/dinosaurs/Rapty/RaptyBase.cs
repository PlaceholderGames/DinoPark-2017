using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptyBase : MonoBehaviour {
    public Animator myAnim;

    // Use this for initialization
    void Start () {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            myAnim.Play("Attack", -1, 0f);
        }
        if (Input.GetKeyDown("k"))
        {
            myAnim.Play("AlertedState", -1, 0f);
        }
        if (Input.GetKeyDown("l"))
        {
            myAnim.Play("Walk", -1, 0f);
        }
        if (Input.GetKeyUp("l"))
        {
            myAnim.Play("Idle", -1, 0f);
        }

    }
}
