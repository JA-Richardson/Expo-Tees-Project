using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject lightTurretPrefab;
    public GameObject heavyTurretPrefab;
    public GameObject normalTurretPrefab;
    private GameObject turretPrefab;
    private GameObject turretInstance;
    private bool isPositionValid;
    private int terrainLayer;
    public int Slot;

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
            BoxCollider turretCollider = turretInstance.GetComponent<BoxCollider>();
            Collider[] colliders = Physics.OverlapBox(turretInstance.transform.position, turretCollider.bounds.extents, turretInstance.transform.rotation, LayerMask.GetMask("Tower"));
            isPositionValid = colliders.Length == 1;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPositionValid)
        {
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
            Destroy(gameObject);
        }
    }
}