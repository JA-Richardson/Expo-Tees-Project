using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    int coolDown = 7;
    float coolDownCount = 0;
    int AmountOfEnemiesToSpawn = 5;
    int AmountOfEnemiesSpawned = 0;
    public GameObject HeavyEnemy;
    public GameObject NormalEnemy;
    public GameObject LightEnemy;
    bool newRound = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AmountOfEnemiesSpawned < AmountOfEnemiesToSpawn)
        {
            if (coolDownCount == 0)
            {
                Instantiate(NormalEnemy, gameObject.transform);
                AmountOfEnemiesSpawned++;
                newRound = false;
            }
            coolDownCount = coolDownCount + (1 * Time.deltaTime);
            if (coolDownCount >= coolDown)
            {
                coolDownCount = 0.0f;
            }
        }
        
        if(GameManager.Instance.CheckRoundEnded() && !newRound)
        {
            GameManager.Instance.roundNumber++;
            AmountOfEnemiesSpawned = 0;
            AmountOfEnemiesToSpawn += 2;
            coolDown -= 1;
            newRound = true;
            GameManager.Instance.NewCards();
        }
    }
}
