using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullSoundHeat
{

    public static float heat = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (heat > 0)
            heat -= Time.deltaTime;
    }
}