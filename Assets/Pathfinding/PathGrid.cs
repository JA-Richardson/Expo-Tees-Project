using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PathGrid : MonoBehaviour
{

    public bool onlyDisplayPathGizmos;
    public float nodeRadius;
    PathNode[,] grid;
    readonly Stopwatch timer = new();
    public TerrainType[] terrainPenalty;
    readonly Dictionary<int, int> walkableRegionsDictionary = new();
    LayerMask walkableMask;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    private int penaltyMax = int.MinValue;
    private int penaltyMin = int.MaxValue;
    public int obstacleProximityPenalty = 25;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        //Loop to add all the terrain types to the walkable mask
        foreach (TerrainType terrain in terrainPenalty)
        {
            walkableMask.value |= terrain.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(terrain.terrainMask.value, 2), terrain.terrainPenalty);
        }
        CreateGrid();
        timer.Start();
    }

    void Update()
    {
        //rebuilds the grid every second
        if (timer.ElapsedMilliseconds > 1000)
        {
            timer.Reset();
            timer.Start();
            CreateGrid();
        }
        //UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    //Function to create the grid, using the world size and node radius
    //Applies the penalty to the nodes based on the terrain type
    //Then applies the blur
    void CreateGrid()
    {
        grid = new PathNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        // Double for loop creates the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementPenalty = 0;

                Ray ray = new(worldPoint + Vector3.up * 50, Vector3.down); // Raycast from above the node down to the ground
                if (Physics.Raycast(ray, out RaycastHit hit, 100, walkableMask)) // If the raycast hits something
                {

                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty); // Get the movement penalty for the terrain type

                }
                if (!walkable) // If the node is not walkable
                {
                    movementPenalty += obstacleProximityPenalty;
                }



                grid[x, y] = new PathNode(walkable, worldPoint, x, y, movementPenalty); // Create the node
            }
        }
        Blur(3); // Apply the blur
    }

    //returns the neighbouring nodes of a node in the grid
    public List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    //Gets the node from a world position
    public PathNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));


        if (grid != null && !onlyDisplayPathGizmos)
        {
            foreach (PathNode n in grid)
            {

                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));

                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;

                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));
            }
        }

    }

    //Applies a box blur to the grid for smoother paths
    //Uses separate horizontal and vertical passes for improved efficiency
    void Blur(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelScope = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelScope; x <= kernelScope; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelScope);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelScope - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelScope, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelScope; y <= kernelScope; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelScope);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelScope - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelScope, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }
}

[System.Serializable]
public class TerrainType
{
    public LayerMask terrainMask;
    public int terrainPenalty;
}