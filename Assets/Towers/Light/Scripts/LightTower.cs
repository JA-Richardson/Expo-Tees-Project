using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : BaseTowerCode
{
    private void Awake()
    {
        TowerSetup(10, 2f, 15);
    }
}
