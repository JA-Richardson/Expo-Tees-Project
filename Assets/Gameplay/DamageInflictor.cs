using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInflictor : MonoBehaviour
{
    public Vector3 velocity = new Vector3(0,0,1);
    private float speed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            Destroy(gameObject);
        }
    }
}
