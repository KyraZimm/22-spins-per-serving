using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float damage;

    float maxLifetime = 100f;
    float age = 0f;

    void Update()
    {
        age += Time.deltaTime;
        if (age > maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Bullets will only be destroyed when they collide with tangible objects (assuming that some objects will be intangible)
        Boss bossQuery = other.GetComponentInParent<Boss>();
        if (bossQuery != null) {
            bossQuery.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
