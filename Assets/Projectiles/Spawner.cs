using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] bool enable = false;
    [SerializeField] float fireDelay = 1.0f;
    [SerializeField] float turnRate = Mathf.PI / 4.0f;
    [SerializeField] float projectileSpeed = 1.0f;

    private float angle = 0.0f;
    private float timeSinceLastSpawn = 0.0f;

    private float minFireDelay = 0.01f;

    private void Update()
    {
        fireDelay = Mathf.Max(minFireDelay, fireDelay);

        timeSinceLastSpawn += Time.deltaTime;
        while (timeSinceLastSpawn >= fireDelay)
        {
            if (enable)
            {
                float timeOffset = timeSinceLastSpawn - fireDelay;
                float spawnAngle = angle - (turnRate * timeOffset);
                SpawnProjectile(spawnAngle, timeOffset);
            }

            timeSinceLastSpawn -= fireDelay;
        }

        angle += turnRate * Time.deltaTime;
        angle %= 2.0f * Mathf.PI;
    }

    private void SpawnProjectile(float angle, float age)
    {
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 offset = dir * projectileSpeed * age;

        GameObject projectile = Instantiate(
            prefab,
            transform.position + (Vector3)offset,
            Quaternion.identity
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = dir * projectileSpeed;
    }
}
