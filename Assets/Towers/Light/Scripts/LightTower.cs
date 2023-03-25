using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : BaseTowerCode
{
    private void Awake()
    {
        AttackPower = 10;
        AttackSpeed = 2f;
        TowerRange = 15;
    }
}
