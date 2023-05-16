using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public float cellSize = 1f; 
    public Vector2Int gridSize = new Vector2Int(10, 10);
    public GameObject prefab;
    public Vector2Int cell;

    private GameObject[,] gridOccupiedObjects;
    public GameObject[] wallPrefabs;

    [System.Serializable]
    public class TowerData
    {
        public Vector2Int cell;
        public GameObject tower;
    }

    public List<TowerData> towerDataList = new List<TowerData>();

    void Start()
    {
        if (gridOccupiedObjects == null)
        {
            gridOccupiedObjects = new GameObject[gridSize.x, gridSize.y];
        }

        foreach (TowerData towerData in towerDataList)
        {
            gridOccupiedObjects[towerData.cell.x, towerData.cell.y] = towerData.tower;
        }
    }
    [ContextMenu("Clean Up Tower Data")]
    public void CleanUpTowerData()
    {
        for (int i = towerDataList.Count - 1; i >= 0; i--)
        {
            if (towerDataList[i].tower == null)
            {
                towerDataList.RemoveAt(i);
            }
        }
    }
    // This method converts a world position to a grid position
    public Vector3 WorldToGridPosition(Vector3 worldPosition)
    {
        float x = Mathf.Round((worldPosition.x - transform.position.x) / cellSize) * cellSize + transform.position.x;
        float z = Mathf.Round((worldPosition.z - transform.position.z) / cellSize) * cellSize + transform.position.z;
        return new Vector3(x, worldPosition.y, z);
    }
    public Vector2Int WorldToCell(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - transform.position.x) / cellSize);
        int z = Mathf.FloorToInt((worldPosition.z - transform.position.z) / cellSize);
        return new Vector2Int(x, z);
    }
    public Vector3 CellToWorld(Vector2Int cell)
    {
        float x = (cell.x * cellSize) + transform.position.x;
        float z = (cell.y * cellSize) + transform.position.z;
        return new Vector3(x, 0, z); 
    }
    public bool IsCellOccupied(Vector3 worldPosition)
    {
        Vector2Int cell = WorldToCell(worldPosition);
        if (cell.x >= 0 && cell.x < gridSize.x && cell.y >= 0 && cell.y < gridSize.y && gridOccupiedObjects[cell.x, cell.y] != null)
        {
            return true;
        }
        return false;
    }


    // Draw the grid 
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int z = 0; z <= gridSize.y; z++)
            {
                var point = transform.position + new Vector3(x * cellSize, 0, z * cellSize);
                Gizmos.DrawWireCube(point, new Vector3(cellSize, 0, cellSize));
            }
        }
    }

    // This method checks if a position is within the grid bounds
    public bool IsPositionInGrid(Vector3 position)
    {
        Vector3 gridPos = WorldToGridPosition(position);
        return gridPos.x >= transform.position.x && gridPos.x <= transform.position.x + gridSize.x * cellSize &&
               gridPos.z >= transform.position.z && gridPos.z <= transform.position.z + gridSize.y * cellSize;
    }
    public void addObjectToGrid(Vector2Int cell, GameObject gameobject)
    {

        if (gridOccupiedObjects[cell.x, cell.y] != null)
        {
            return ;
        }

        gridOccupiedObjects[cell.x, cell.y] = gameobject;

        
    }
    public void AddPrefab()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned");
            return;
        }

        if (Terrain.activeTerrain == null)
        {
            Debug.LogError("No active Terrain found");
            return;
        }

        if (cell.x >= 0 && cell.x <= gridSize.x && cell.y >= 0 && cell.y <= gridSize.y)
        {
            if (gridOccupiedObjects == null)
            {
                gridOccupiedObjects = new GameObject[gridSize.x, gridSize.y];
            }

            if (gridOccupiedObjects[cell.x, cell.y] == null)
            {
                Vector3 worldPos = CellToWorld(cell);
                worldPos.y = Terrain.activeTerrain.SampleHeight(worldPos);
                GameObject instance = Instantiate(prefab, worldPos, Quaternion.identity);
                gridOccupiedObjects[cell.x, cell.y] = instance;
                SpawnWallIfAdjacent(cell);
                TowerData newTowerData = new TowerData();
                newTowerData.cell = cell;
                newTowerData.tower = instance;
                towerDataList.Add(newTowerData);
            }
            else
            {
                Debug.LogError("Cell is already occupied");
            }
        }
        else
        {
            Debug.LogError("Cell is out of grid bounds");
        }
    }

    public void SpawnWallIfAdjacent(Vector2Int cellPos)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(0, 1), // Above
        new Vector2Int(0, -1), // Below
        new Vector2Int(-1, 0), // Left
        new Vector2Int(1, 0), // Right
        };

        foreach (var dir in directions)
        {
            Vector2Int adjacentCell = cellPos + dir;
            Vector3 adjacentWorldPos = CellToWorld(adjacentCell);
            if (IsCellOccupied(adjacentWorldPos))
            {
                Vector3 wallPos = (CellToWorld(cellPos) + adjacentWorldPos) / 2;
                float height1 = Terrain.activeTerrain.SampleHeight(CellToWorld(cellPos));
                float height2 = Terrain.activeTerrain.SampleHeight(adjacentWorldPos);
                wallPos.y = (height1 + height2) / 2;

                // Randomly select a wall prefab
                GameObject wallPrefab = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
                GameObject wallInstance = Instantiate(wallPrefab, wallPos, Quaternion.identity);

                float originalWallLength = 5f; // Set this to the original length of the wall along the X-axis
                float scale = cellSize / originalWallLength;
                wallInstance.transform.localScale = new Vector3(scale, scale, scale);

                // Rotate wall if necessary
                if (dir.y != 0) // If the towers are vertically adjacent
                {
                    wallInstance.transform.Rotate(0, 90, 0); // Rotate 90 degrees around Y-axis
                }
            }
        }
    }
}
