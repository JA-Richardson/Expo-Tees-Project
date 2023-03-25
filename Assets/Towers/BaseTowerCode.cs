using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerCode : MonoBehaviour
{
    protected int AttackPower;
    protected float AttackSpeed;
    protected float TimeSinceLastAttack;
    protected int TowerRange;

    public virtual void Updateloop(){}

    protected bool EnemyInRange()
    {
        return true;
    }

    protected bool AttackOffCooldown()
    {
        return true;
    }

    protected void Attack()
    {

    }
}
