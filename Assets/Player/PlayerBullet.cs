using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Projectile
{
    private float maxLifetime = 3.0f;
    private float age = 0.0f;

    void Update()
    {
        age += Time.deltaTime;
        if (age > maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {

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
