using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseTowerCode : MonoBehaviour
{
    protected int AttackPower;
    protected float AttackSpeed;
    protected float EndOfAttackCooldown;
    protected int TowerRange;
    protected EnemyBaseClass Target;
    public LayerMask TargetLayer;
    public LayerMask Obstructions;
    public bool EnemiesInRange;
    float VisAngle = 360f;

    //Attacks enemies in tower range.
    public virtual void Updateloop()
    {
        if(EnemiesInRange)
        {
            if(AttackOffCooldown())
            {
                Attack();
            }
        }
    }

    //Makes the thing that I don't have a clue turn like it was on update.
    private void Awake()
    {
        StartCoroutine(FOV_Run());
    }

    //Have no clue
    private IEnumerator FOV_Run()
    {
        float Wait_Time = 0.5f;
        WaitForSeconds Delay = new WaitForSeconds(Wait_Time);

        while (true)
        {
            FOV_Check();
            yield return Delay;

        }
    }

    //Looks for enemies in towers range
    private void FOV_Check()
    {
        Collider[] Vision = Physics.OverlapSphere(transform.position, TowerRange, TargetLayer);
        if (Vision.Length != 0)
        {
            print("EnemiesInRange");
            Vector3 Target_Pos = Vision[0].transform.position;
            Vector3 Target_Direction = (Vision[0].transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, Target_Direction) < VisAngle / 2)
            {
                print("Passed if 1");
                float Distance_to_Target = Vector3.Distance(transform.position, Target_Pos);
                if (!Physics.Raycast(transform.position, Target_Direction, Distance_to_Target, Obstructions))
                {
                    print("Passed if 2");
                    EnemiesInRange = true;
                    Target = Vision[0].gameObject.GetComponent<EnemyBaseClass>();
                }
                else
                {
                    print("Failed if 2");
                    EnemiesInRange = false;
                }
            }
            else
            {
                print("Failed if 1");
                EnemiesInRange = false;
            }

        }
        else if (EnemiesInRange)
        {
            EnemiesInRange = false;
        }
        else
        {
            print("No enemies could not be found");
        }

    }

    //Stops tower from attacking when on cooldown.
    protected bool AttackOffCooldown()
    {
        if(Time.time >= EndOfAttackCooldown)
        {
            return true;
        }
        return false;
    }

    protected void Attack()
    {
        Target.TakeDamage(AttackPower);
        EndOfAttackCooldown = Time.time + AttackSpeed;
    }

    //Sets the towers values.
    protected void TowerSetup(int power, float speed, int range)
    {
        AttackPower = power;
        AttackSpeed = speed;
        TowerRange = range;
    }
}
