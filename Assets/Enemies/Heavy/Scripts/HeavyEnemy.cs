using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : EnemyBaseClass
{
    void Awake()
    {
        SetUpEnemy(150, 10, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
