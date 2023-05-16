using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyBaseClass : MonoBehaviour
{
    public int Health;
    public int Speed;
    public int Power;
    Transform target;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Base").transform;
        agent.SetDestination(target.position);
        agent.speed = Speed;
    }

    void Update()
    {

    }

    public void checkCollision()
    {
        float distanceToTarget = (target.position - transform.position).magnitude;
        if (distanceToTarget <= 35)
        {
            GameManager.Instance.BaseHealth -= 5;
            Destroy(gameObject);
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if(Health < 0)
        {
            Destroy(gameObject);
        }
    }

    protected void SetUpEnemy(int health, int speed, int power)
    {
        Health = health;
        Speed = speed;
        Power = power;
    }
}