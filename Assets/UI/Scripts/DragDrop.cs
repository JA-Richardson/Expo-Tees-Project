using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject turretPrefab;
    private GameObject turretInstance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        turretInstance = Instantiate(turretPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            turretInstance.transform.position = hit.point;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        turretInstance = null; // If you want the turret to stay after you stop dragging
    }
}