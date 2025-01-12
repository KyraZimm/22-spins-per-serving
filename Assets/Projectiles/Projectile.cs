using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;

    protected Vector2 xRange = new Vector2(-11f, 11f);
    protected Vector2 yRange = new Vector2(-6f, 6f);

    protected float maxLifetime = 100.0f;
    protected float age = 0.0f;

    protected BulletPool parentPool;

    public void Init(BulletPool createdByPool) {
        parentPool = createdByPool;
        gameObject.SetActive(false);
    }

    void Update()
    {
        age += Time.deltaTime;
        if (age > maxLifetime)
        {
            Despawn();
        }

        if (transform.position.x < xRange.x || transform.position.x > xRange.y) Despawn();
        else if (transform.position.y < yRange.x || transform.position.y > yRange.y) Despawn();
    }

    public void Spawn(Vector2 spawnPos, Vector2 velocity) {
        transform.position = spawnPos;
        rb.velocity = velocity;
    }
    public void Despawn() {
        parentPool.Despawn(this);
    }

    //NOTE: projectiles are currently being despawned by boss attacks and by player contact. Generic "Destroy" commands may confuse existing bullet pools
    /*private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }*/
}
