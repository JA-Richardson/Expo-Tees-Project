using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAroundTower : BaseTowerCode
{
    private void Awake()
    {
        TowerSetup(25, 5f, 20);
    }

    private void Update()
    {
        if (EnemiesInRange)
        {
            if (AttackOffCooldown())
            {
                Attack();
            }
        }
    }
}
