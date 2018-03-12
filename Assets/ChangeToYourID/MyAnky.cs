using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAnky : Agent
{
    public enum ankyState
    {
        IDLE,
        EATING, // This is for eating and drinking, depending on y value of the object
        ALERT,  //
        ATTACKING,
        FLEEING  
    };
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

    }

}
