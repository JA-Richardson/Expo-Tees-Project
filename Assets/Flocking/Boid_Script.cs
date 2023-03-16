using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid_Script : MonoBehaviour
{

    public Quaternion direction;
    public Vector3 position;
    float speed = 10;
    float Turningrate = 3;
    float turnRandomisation = 1;
    float spawnRange = 25;
    float timer = 0;
    

    public List<GameObject> neighbours = new List<GameObject>();
    
    

    // Start is called before the first frame update
    void Start()
    {
        direction = Random.rotation;
        this.transform.position = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
        this.transform.rotation = direction;    
    }
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;


        if (neighbours.Count>0)
        {
            if (timer > 0.5)
            {
                direction = allignment();
                timer = 0;
            }

        }
        
       


        
        
        //adds some random variation to the agents direction 
        direction = direction * Quaternion.Euler(new Vector3(Random.Range(-turnRandomisation, turnRandomisation), Random.Range(-turnRandomisation, turnRandomisation), Random.Range(-turnRandomisation, turnRandomisation)));

        // slowly turns the agent to point towards the desired direction
        transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * Turningrate);

        //moves the agent forward 
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
    }
    Quaternion allignment()
    {

        Quaternion average = new Quaternion(0, 0, 0, 0);
        int amount = 0;

        foreach (var neighbour in neighbours)
        {
            amount++;

            Quaternion rotation = neighbour.transform.rotation;

            average = Quaternion.Slerp(average, rotation, 1 / amount);
        }
        return average;
    }
    Vector3 cohesion()
    {
        Vector3 average = new Vector3(0, 0, 0);

        foreach (var neighbour in neighbours)
        {

            average += neighbour.transform.position;
            

            
        }
        average /= neighbours.Count;

        Vector3 vectorToAverage = average - this.transform.position;

        

        return vectorToAverage;

    }
    void separation()
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boid")
        {
            neighbours.Add(other.gameObject);
                    Debug.Log("trigger!");
        }
        
        
    }
    

    private void OnTriggerExit(Collider other)
    {
        neighbours.Remove(other.gameObject);
    }
}
