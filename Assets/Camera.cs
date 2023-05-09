using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float moveSpeed;
    public float moveTime;
    public float rotAmount;
    public Vector3 zoomAmount;

    public Vector3 newPos;
    public Quaternion newRot;
    public Vector3 newZoom;

    public Vector3 dragStart;
    public Vector3 dragCurrent;
    public Vector3 rotStart;
    public Vector3 rotCurrent;

    private void Awake()
    {
        cameraTransform = UnityEngine.Camera.main.transform;
    }
    // Start is called before the first frame update

    //private void Awake()
    //{
    //    cameraTransform = UnityEngine.Camera.main.transform;
    //}
    void Start()
    {
        newPos = transform.position;
        newRot = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInputHandler();
        MouseInputHandler();
    }

    void MouseInputHandler()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStart = ray.GetPoint(entry);
            }
        }
        if(Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrent = ray.GetPoint(entry);
                newPos = transform.position + dragStart - dragCurrent;
            }
        }
        if(Input.GetMouseButtonDown(2))
        {
            rotStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotCurrent = Input.mousePosition;
            Vector3 rotDiff = rotStart - rotCurrent;
            rotStart = rotCurrent;

            newRot *= Quaternion.Euler(Vector3.up * (-rotDiff.x / 5f));
        }
    }
    
    void KeyboardInputHandler()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = fastSpeed;
        }
        else
        {
            moveSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPos += Vector3.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPos += Vector3.back * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPos += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPos += Vector3.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            newRot *= Quaternion.Euler(Vector3.up * rotAmount * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRot *= Quaternion.Euler(Vector3.up * -rotAmount * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount * Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, newPos, moveTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, moveTime * Time.deltaTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, moveTime * Time.deltaTime);

    }
}
