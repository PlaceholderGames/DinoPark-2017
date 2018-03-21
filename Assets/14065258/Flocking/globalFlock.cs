using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour {

    public GameObject dinoPrefab;
    public GameObject goalPrefab;
    public static int sizeXMin = 1400;
    public static int sizeXMax = 1440;
    public static int sizeY = 114;
    public static int sizeZMin = 830;
    public static int sizeZMax = 855;

    static int numDino = 10;
    public static GameObject[] allDino = new GameObject[numDino];

    public static Vector3 goalPos = new Vector3(sizeXMax - sizeXMin, 122, sizeZMax - sizeZMin);

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numDino; i++)
        {
            Vector3 pos = new Vector3(Random.Range(sizeXMin, sizeXMax),
                                                   sizeY ,    
                                      Random.Range(sizeZMin, sizeZMax));

            allDino[i] = (GameObject)Instantiate(dinoPrefab, pos, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Random.Range(0,10000) < 50)
        {
            goalPos = new Vector3(Random.Range(sizeXMax+100, sizeXMin-100), sizeY+20, Random.Range(sizeZMax+100, sizeZMin-100));

            goalPrefab.transform.position = goalPos;
        }
	}
}
