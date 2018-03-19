using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TimeOfDay : MonoBehaviour
{

    [SerializeField] public Light sun;
    [SerializeField] public float secondsInFullDay = 120f;

    [Range(0, 1)]
    [SerializeField]
    public float currentTimeOfDay = 0;
    private float timeMultiplier = 1f;
    private float sunInitialIntesity;

    void Start()
    {
        sunInitialIntesity = sun.intensity;
    }

    void Update()
    {


        UpdateSun();

        // UpdateSun();

        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
    }





    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        float intesityMultiplier = 1;

        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intesityMultiplier = 0;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intesityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }

        else if (currentTimeOfDay >= 0.73f)
        {
            intesityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }

        sun.intensity = sunInitialIntesity * intesityMultiplier;
    }



}