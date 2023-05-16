using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WallCardScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image blankImage;
    public Sprite WallCard;
    int WallCardsRemaining = 3;
    public Grid grid;
    public GameObject WallTower;
    private GameObject turretInstance;
    private int terrainLayer;
    private bool isPositionValid;
    bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        terrainLayer = LayerMask.GetMask("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        if(WallCardsRemaining > 0)
        {
            blankImage.enabled = true;
            blankImage.sprite = WallCard;
            active = true;

        }
        else if (WallCardsRemaining <= 0)
        {
            blankImage.enabled = false;
            active = false;
            if (GameManager.Instance.CheckRoundEnded())
            {
                WallCardsRemaining = 3;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(active)
        {
            turretInstance = Instantiate(WallTower);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            turretInstance.transform.position = hit.point;
            Vector3 gridPosition = grid.WorldToGridPosition(hit.point);
            gridPosition.y -= 5;
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
            Vector2Int cellPos = grid.WorldToCell(turretInstance.transform.position);

            grid.addObjectToGrid(cellPos, turretInstance);
            grid.SpawnWallIfAdjacent(cellPos);
            turretInstance = null;
            WallCardsRemaining -= 1;
        }
        else
        {
            Destroy(turretInstance);
            turretInstance = null;
        }

    }

}
