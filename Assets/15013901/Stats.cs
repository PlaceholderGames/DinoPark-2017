using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int Health = 200;
    public int Food = 100;
    public int Water = 100;
    public GameObject Anky;
    float timer;

    void HungerDecrease()
    {
        if (transform.position.y <= 62)
        {
            Food -= 1;

        }
        else
        {
            Food += 5;
            if(Food >= 100)
            {
                Debug.Log("Food is Full");
            }
        }
        Debug.Log(Food);
    }
    void NeedWater()
    {

        if (transform.position.y >= 37)
        {
            Water -= 1;
            

        }
        else
        {
            Water += 5;

            if (Water >= 100)
            {
                Debug.Log("Water is Full");
            }
        }

    }   

}





