using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : BaseTowerCode
{
    private void Awake()
    {
        TowerSetup(10, 2f, 50);
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
