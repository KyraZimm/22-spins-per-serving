using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note for future ref: every level of inheritance in Unity creates additional complexity on the C++ layer
// Unless inheritance is required or is really convenient for code organization, it's usually better to use a standalone class or derive directly from MonoBehaviour
public class PlayerBullet : Projectile // <- we'll also be calling different checks on PlayerBullets vs. Projectiles like 99% of the time, so we should prob make PlayerBullet a separate class. To avoid weird edge cases like PlayerBullets getting compared to Projectile BulletPools
{
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
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
