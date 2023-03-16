using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;


    // Start is called before the first frame update
    void Start()
    {
        //call "Tick" every five seconds
        InvokeRepeating("Tick", 5, 5);
    }

    void Tick()
    {
        //do whatever you want, in here
        //simple log statement for demo purposes
        Instantiate(enemy, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
