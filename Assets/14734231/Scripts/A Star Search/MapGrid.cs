using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGrid : MonoBehaviour
{

    public Vector2 gridWorldSize; // Size of the map grid
    public float tileSize = 1.0f;  // Size of each tile
    public float heightThreshold = 1.0f; // The maximum height distance between each corner of a tile to determine if walkable
    public float seaLevel = 0.0f; // Minimum walkable height

    [HideInInspector]
    public MapTile[,] tiles;
    [HideInInspector]
    public List<MapTile> waterEdge = new List<MapTile>();
    public List<MapTile> foodEdge = new List<MapTile>();

    private Terrain terrain;

    private float oldTileSize;
    private float oldHeightThreshold;
    private float oldSeaLevel;


    // Use this for initialization
    void Start()
    {
        terrain = Terrain.activeTerrain;

        if (tileSize == 0) // Exit with error if node size is zero
        {
            Debug.LogError("A* error: Node size cannot be zero!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        initialiseTiles();
        checkWalkable();

        oldTileSize = tileSize;
        oldHeightThreshold = heightThreshold;
        oldSeaLevel = seaLevel;
    }

    private void initialiseTiles()
    {
        Vector2 tileCount = new Vector2((int)(gridWorldSize.x / tileSize), (int)(gridWorldSize.y / tileSize)); // Find how many tiles fit in the map

        tiles = new MapTile[(int)tileCount.x, (int)tileCount.y]; // Initialise tiles array

        // Find bottom left of map
        Vector3 mapBottomLeft = transform.position + (Vector3.left * gridWorldSize.x / 2) + (Vector3.back * gridWorldSize.y / 2);

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // Increment from bottom left across x and y axis for each new tile 
                Vector3 tileLocation = mapBottomLeft + Vector3.right * (x * tileSize + (tileSize / 2)) + Vector3.forward * (y * tileSize + (tileSize / 2));

                tileLocation.y = terrain.SampleHeight(tileLocation);

                // Create new tile at tileLocation 
                tiles[x, y] = new MapTile(tileLocation);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Only check which tiles are walkable if a variable has changed
        if (oldTileSize != tileSize)
            oldTileSize = tileSize;
        else if (oldHeightThreshold != heightThreshold)
            oldHeightThreshold = heightThreshold;
        else if (oldSeaLevel != seaLevel)
            oldSeaLevel = seaLevel;
        else
            return;

        checkWalkable();
    }


    public void checkWalkable()
    {
        terrain = Terrain.activeTerrain;

        for (int x = 0; x < tiles.GetLength(0); x++) // Loop through nodes
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Vector3 tileLocation = tiles[x, y].position; // Get the position

                Vector3[] points = new Vector3[4]; // Array of positions at four corners of tile
                points[0] = new Vector3(tileLocation.x - (tileSize / 2), tileLocation.y, tileLocation.z + (tileSize / 2)); // Top left
                points[1] = new Vector3(tileLocation.x + (tileSize / 2), tileLocation.y, tileLocation.z + (tileSize / 2)); // Top right
                points[2] = new Vector3(tileLocation.x + (tileSize / 2), tileLocation.y, tileLocation.z - (tileSize / 2)); // Bottom right
                points[3] = new Vector3(tileLocation.x - (tileSize / 2), tileLocation.y, tileLocation.z - (tileSize / 2)); // Bottom left

                float[] heightPoints = new float[4]; // Height of all corners

                for (int i = 0; i < 4; i++)
                    heightPoints[i] = terrain.SampleHeight(points[i]);


                bool walkable = true;

                if (tileLocation.y < seaLevel) // If tile is under sea, unwalkable
                    walkable = false;

                for (int i = 0; i < 4 && walkable; i++) // Loop through corners
                {
                    float diff = Mathf.Abs(tileLocation.y - heightPoints[i]); // Work out difference between corner and centre

                    if (diff > heightThreshold) // If corner too high, unwalkable
                        walkable = false;
                }

                tiles[x, y].walkable = walkable;
                if (tiles[x, y].walkable == false)
                {
                    waterEdge.Add(tiles[x, y]);
                }

                if (tiles[x, y].position.y > 70 && tiles[x, y].position.y < 80)
                {
                    foodEdge.Add(tiles[x, y]);
                }

            }
        }
    }

    /// <summary>
    /// Returns the corresponding mapTile from a position
    /// </summary>
    /// <param name="position">A vector3 world position</param>
    /// <returns>The corresponding mapTile at position</returns>
    public MapTile getTileFromPosition(Vector3 position)
    {
        Vector2 coord = getCoordFromPosition(position);

        // Return tile from array
        return tiles[(int)coord.x, (int)coord.y];
    }

    /// <summary>
    /// Converts a vector 3 world position into a vector2 grid coordinate
    /// </summary>
    /// <param name="position">Vector 3 world position</param>
    /// <returns>Vector 2 grid coordinate</returns>
    public Vector2 getCoordFromPosition(Vector3 position)
    {
        position -= transform.position;
        Vector2 mapPercentage; // Convert the position to a percentage across the available map (clamped between 0% and 100%).
        mapPercentage.x = Mathf.Clamp01((position.x + gridWorldSize.x / 2) / gridWorldSize.x);
        mapPercentage.y = Mathf.Clamp01((position.z + gridWorldSize.y / 2) / gridWorldSize.y);

        // Advance across tiles array for percentage, then round to closest tile
        int xTile = Mathf.RoundToInt((tiles.GetLength(0) - 1) * mapPercentage.x);
        int yTile = Mathf.RoundToInt((tiles.GetLength(1) - 1) * mapPercentage.y);

        //        0%=================100%
        // Array [0] [1] [2] [3] [4] [5]

        return new Vector2(xTile, yTile);
    }

    public MapTile getEdgeWater(GameObject dino)
    {
        int chosenTile = 0;
        float shortest = 0;
        for (int i = 0; i < waterEdge.Count; i++)
        {
            if (shortest == 0)
            {
                shortest = Vector3.Distance(dino.transform.position, waterEdge[i].position);
                chosenTile = i;
            }
            else if (shortest > Vector3.Distance(dino.transform.position, waterEdge[i].position))
            {
                shortest = Vector3.Distance(dino.transform.position, waterEdge[i].position);
                chosenTile = i;
            }
        }
        return waterEdge[chosenTile];
    }

    public MapTile getFood(GameObject dino)
    {
        int chosenTile = 0;
        float shortest = 0;

        for (int i = 0; i < foodEdge.Count; i++)
        {
            if (shortest == 0)
            {
                shortest = Vector3.Distance(dino.transform.position, foodEdge[i].position);
                chosenTile = i;
            }
            else if (shortest > Vector3.Distance(dino.transform.position, foodEdge[i].position))
            {
                shortest = Vector3.Distance(dino.transform.position, foodEdge[i].position);
                chosenTile = i;
            }
        }

        return foodEdge[chosenTile];
    }
}
