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

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: Damage player before despawning
        Destroy(gameObject);
    }
}
