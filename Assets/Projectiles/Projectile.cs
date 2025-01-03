using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 vel;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void Init(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: Damage player before despawning
        Destroy(gameObject);
    }

}
