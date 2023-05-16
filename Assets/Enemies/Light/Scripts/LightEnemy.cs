using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemy : EnemyBaseClass
{

    void Awake()
    {
        SetUpEnemy(75, 25, 25);
    }

    // Update is called once per frame
    void Update()
    {
        checkCollision();
    }
}
