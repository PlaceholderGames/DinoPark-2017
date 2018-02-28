using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graze : MonoBehaviour
{

    public Hunger hungerSO;
    TerrainData terrainData;
    Transform terrainPos;
    float [,,] splatmapData;

    // Use this for initialization
    void Start()
    {
        terrainData = Terrain.activeTerrain.terrainData;
        terrainPos = Terrain.activeTerrain.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        float distance = 100f;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            Transform dinoTransform = hit.collider.transform;
            Vector3 dinoPos = new Vector3(dinoTransform.position.x, dinoTransform.position.y, dinoTransform.position.z);

            float terX = ((dinoPos.x - terrainPos.position.x) / terrainData.size.x) * terrainData.alphamapWidth;
            float terZ = ((dinoPos.z - terrainPos.position.z) / terrainData.size.z) * terrainData.alphamapHeight;

            splatmapData = terrainData.GetAlphamaps((int)dinoPos.x, (int)dinoPos.y, 1, 1);

            float texture1Level = splatmapData[0, 0, 1];
            float texture2Level = splatmapData[0, 0, 2];
            float texture3Level = splatmapData[0, 0, 3];
            float texture4Level = splatmapData[0, 0, 4];

            Debug.Log("T1L" + texture1Level);
            Debug.Log("T2L" + texture2Level);
            Debug.Log("T3L" + texture3Level);
            Debug.Log("T4L" + texture4Level);
        }
    }

}
