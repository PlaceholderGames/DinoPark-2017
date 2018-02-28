using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graze : MonoBehaviour {

    public Hunger hungerSO;
    TerrainData mTerrainData;
    float[,,] mSplatmapData;
    int mNumTextures;

    // Use this for initialization
    void Start () {
        mTerrainData = Terrain.activeTerrain.terrainData;
        int alphamapWidth = mTerrainData.alphamapWidth;
        int alphamapHeight = mTerrainData.alphamapHeight;

        mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        mNumTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);
	}
	
    private Vector3 ConvertToSplatMapCoord(Vector3 dinoPos)
    {
        Vector3 vecRet = new Vector3();
        Terrain ter = Terrain.activeTerrain;
        Vector3 terPos = ter.transform.position;

        vecRet.x = ((dinoPos.x - terPos.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
        vecRet.z = ((dinoPos.z - terPos.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;

        return vecRet;
    }

	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        float distance = 100f;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            int tex = GetActiveTerrainTextureIdx(hit);
            Debug.Log(tex);
        }
    }

    int GetActiveTerrainTextureIdx(RaycastHit hit)
    {
        Vector3 dinoPos = hit.collider.transform.position;
        Vector3 terrainCoord = ConvertToSplatMapCoord(dinoPos);
        int ret = 0;
        float comp = 0f;

        for (int i = 0; i < mNumTextures; i++)
        {
            if (comp < mSplatmapData[(int)terrainCoord.z, (int)terrainCoord.x, i])
                ret = i;
        }

        return ret;
    }
}
