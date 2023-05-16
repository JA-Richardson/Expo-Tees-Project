using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletScript : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        //GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        //Destroy(effectIns, 2f);

        //EnemyScript enemy = target.gameObject.GetComponent<EnemyScript>();
        //enemy.Damage(1.0f);

        //Destroy(target.gameObject);
        Destroy(gameObject);
    }
}
