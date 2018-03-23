using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drinking : MonoBehaviour
{
    MyAnky myAnky;
    void Awake()
    {
        myAnky = GetComponent<MyAnky>();
    }
    // Update is called once per frame
    public void Drink()
    {
        myAnky.health++;
    }
}
