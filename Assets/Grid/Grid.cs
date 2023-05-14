using UnityEngine;

public class Grid : MonoBehaviour
{
    public float cellSize = 1f; 
    public Vector2Int gridSize = new Vector2Int(10, 10);

    private bool[,] cellOccupied;

    void Start()
    {
        cellOccupied = new bool[gridSize.x, gridSize.y];
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
    public bool IsCellOccupied(Vector3 worldPosition)
    {
        Vector2Int cell = WorldToCell(worldPosition);
        return cellOccupied[cell.x, cell.y];
    }
    public void SetCellOccupied(Vector3 worldPosition, bool occupied)
    {
        Vector2Int cell = WorldToCell(worldPosition);
        cellOccupied[cell.x, cell.y] = occupied;
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
}
