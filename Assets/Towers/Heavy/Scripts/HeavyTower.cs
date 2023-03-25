using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTower : BaseTowerCode
{
    private void Awake()
    {
        AttackPower = 50;
        AttackSpeed = 10f;
        TowerRange = 50;
    }
}
