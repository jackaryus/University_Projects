using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float bulletSpeed = 10;
    public Transform target;

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 direction = target.position - transform.position;
            GetComponent<Rigidbody>().velocity = direction.normalized * bulletSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider co)
    {
        Health health = co.GetComponentInChildren<Health>();
        if (health)
        {
            health.DecreaseHealth();
            Destroy(gameObject);
        }
    }
}
