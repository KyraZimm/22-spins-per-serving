using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    protected float maxLifetime = 100.0f;
    protected float age = 0.0f;

    void Update()
    {
        age += Time.deltaTime;
        if (age > maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    //NOTE: projectiles are currently being despawned by boss attacks and by player contact. Generic "Destroy" commands may confuse existing bullet pools
    /*private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }*/
}
