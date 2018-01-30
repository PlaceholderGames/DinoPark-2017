using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour {

    public Vector2 gridWorldSize;
    public float nodeSize = 1.0f;  
    public float heightThreshold = 1.0f;
    public float seaLevel = 0.0f;

    [HideInInspector]
    public MapTile[,] tiles;

    private Terrain terrain;

    // Use this for initialization
    void Start () {
         terrain = Terrain.activeTerrain;

        if (nodeSize == 0) // Exit with error if node size is zero
        {
            Debug.LogError("A* error: Node size cannot be zero!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        initialiseTiles();
    }
	


	// Update is called once per frame
	void Update () {
        checkWalkable();
	}

    private void initialiseTiles()
    {
        Vector2 tileCount = new Vector2((int)(gridWorldSize.x / nodeSize), (int)(gridWorldSize.y / nodeSize)); // Find how many tiles fit in the map

        tiles = new MapTile[(int)tileCount.x, (int)tileCount.y]; // Initialise tiles array

        // Find bottom left of map
        Vector3 mapBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // Increment from bottom left across x and y axis for each new tile 
                Vector3 tileLocation = mapBottomLeft + Vector3.right * (x * nodeSize + (nodeSize / 2)) + Vector3.forward * (y * nodeSize + (nodeSize / 2));

                // Create new tile at tileLocation 
                tiles[x, y] = new MapTile(tileLocation);

            }
        }
    }

    public void checkWalkable()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Vector3 tileLocation = tiles[x, y].position;
                terrain = Terrain.activeTerrain;

                Vector3[] points = new Vector3[5];
                points[0] = tileLocation;
                points[1] = new Vector3(tileLocation.x - (nodeSize / 2), tileLocation.y, tileLocation.z + (nodeSize / 2));
                points[2] = new Vector3(tileLocation.x + (nodeSize / 2), tileLocation.y, tileLocation.z + (nodeSize / 2));
                points[3] = new Vector3(tileLocation.x + (nodeSize / 2), tileLocation.y, tileLocation.z - (nodeSize / 2));
                points[4] = new Vector3(tileLocation.x - (nodeSize / 2), tileLocation.y, tileLocation.z - (nodeSize / 2));

                float[] heightPoints = new float[5];

                for (int i = 0; i < 5; i++)
                    heightPoints[i] = terrain.SampleHeight(points[i]);

                float summedHeight = 0.0f;
                for (int i = 0; i < 5; i++)
                    summedHeight += heightPoints[i];


                if (Mathf.Abs(summedHeight) > heightThreshold || Mathf.Abs(summedHeight) < seaLevel)
                    tiles[x, y].walkable = false;
                else
                    tiles[x, y].walkable = true;
            }
        }
    }
    
}
