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
    public void Drink()
    {
        // add a condtion to stop when staming reach certian lvl
        myAnky.stamina++;
    }
}
