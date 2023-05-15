using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject lightTurretPrefab;
    public GameObject heavyTurretPrefab;
    public GameObject normalTurretPrefab;
    public GameObject standardWallPrefab;

    public GameObject[] wallPrefabs;
    private GameObject turretPrefab;
    private GameObject turretInstance;
    private bool isPositionValid;
    private int terrainLayer;
    public int Slot;
    public Grid grid;
    void Start()
    {
        terrainLayer = LayerMask.GetMask("Terrain"); 
    }

    void Update()
    {
        switch (GameManager.Instance.showCardSlot(Slot))
        {
            case 0:
                turretPrefab = lightTurretPrefab;
                break;

            case 1:
                turretPrefab = normalTurretPrefab;
                break;

            case 2:
                turretPrefab = heavyTurretPrefab;
                break;

            case 3:
                turretPrefab = standardWallPrefab;
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        turretInstance = Instantiate(turretPrefab);

    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            turretInstance.transform.position = hit.point;
            Vector3 gridPosition = grid.WorldToGridPosition(hit.point);
            if (grid.IsPositionInGrid(gridPosition) && !grid.IsCellOccupied(gridPosition))
            {
                turretInstance.transform.position = gridPosition;
                isPositionValid = true;
            }
            else
            {
                turretInstance.transform.position = hit.point + Vector3.up * 50;
                isPositionValid = false;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPositionValid)
        {
            grid.SetCellOccupied(turretInstance.transform.position, true);
            Vector2Int cellPos = grid.WorldToCell(turretInstance.transform.position);
            SpawnWallIfAdjacent(cellPos);
            turretInstance = null;
        }
        else
        {
            Destroy(turretInstance);
            turretInstance = null;
        }

        if (GameManager.Instance.howManyCards > 0)
        {
            GameManager.Instance.useCard(Slot);
            GameManager.Instance.howManyCards = GameManager.Instance.howManyCards - 1;
        }
        else if (GameManager.Instance.howManyCards <= 0)
        {
            gameObject.GetComponent<CardScript>().Slot = 0;
        }
    }
    void SpawnWallIfAdjacent(Vector2Int cellPos)
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
            Vector3 adjacentWorldPos = grid.CellToWorld(adjacentCell);
            if (grid.IsCellOccupied(adjacentWorldPos))
            {
                Vector3 wallPos = (grid.CellToWorld(cellPos) + adjacentWorldPos) / 2;
                wallPos.y = Terrain.activeTerrain.SampleHeight(wallPos);

                // Randomly select a wall prefab
                GameObject wallPrefab = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
                GameObject wallInstance = Instantiate(wallPrefab, wallPos, Quaternion.identity);

                float originalWallLength = 5f; // Set this to the original length of the wall along the X-axis
                float scale = grid.cellSize / originalWallLength;
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