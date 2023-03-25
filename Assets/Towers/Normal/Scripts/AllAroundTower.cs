using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAroundTower : BaseTowerCode
{
    private void Awake()
    {
        AttackPower = 25;
        AttackSpeed = 5f;
        TowerRange = 20;
    }
}
