using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public Terrain Terrain;
    public int[,] Details;

    private float grass_time;
    private float grass_ticks = 1.0f;

    // Use this for initialization
    void Start()
    {
        Terrain = GetComponent<Terrain>();

        Details = Terrain.terrainData.GetDetailLayer(0, 0, Terrain.terrainData.detailWidth,
            Terrain.terrainData.detailHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (grass_time > grass_ticks)
        {
            Growth();
            Terrain.terrainData.SetDetailLayer(0, 0, 0, Details);
        }

        grass_time += Time.deltaTime;
    }

    void Growth()
    {
        int z = Random.Range(0, 2000);
        int x = Random.Range(0, 2000);

        if (Terrain.GetComponent<Terrain>().SampleHeight(new Vector3(x, 0, z)) > 35.0f && Details[z, x] < 16)
            Details[z, x] += 1;
        else
            Growth();


        
    }
}