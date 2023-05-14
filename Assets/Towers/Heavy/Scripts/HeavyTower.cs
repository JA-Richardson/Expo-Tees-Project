using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTower : BaseTowerCode
{
    private void Awake()
    {
        TowerSetup(50, 10f, 100);
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
