using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdCentre : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var centre = new Vector3();
        var count = 0;

        foreach (var ankylosaurus in FindObjectsOfType<GameObject>())
        {
            if (ankylosaurus.gameObject.tag == "Anky")
            {
                centre += ankylosaurus.transform.position;
                count++;
            }
        }

        transform.position = centre / count;
    }
}