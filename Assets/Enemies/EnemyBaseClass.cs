using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseClass : MonoBehaviour
{
    int MaxHealth = 100;
    int Health;
    public Transform target;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Health = MaxHealth;

        agent.SetDestination(target.position);
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
