using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkSC : MonoBehaviour
{
    MyAnky myAnky;
    void Awake()
    {
        myAnky = GetComponent<MyAnky>();
    }
    // Update is called once per frame
    public void drink()
    {
        myAnky.health++;
    }
}
