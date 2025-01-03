using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Projectile Instantiation")]
    [SerializeField] GameObject prefab;
    [SerializeField] int startingPoolSize;
    [SerializeField] float deactivateBulletDistance;

    [Header("Fire Settings")]
    [SerializeField] bool enable = false;
    [SerializeField] float fireDelay = 1.0f;
    [SerializeField] float turnRate = Mathf.PI / 4.0f;
    [SerializeField] float projectileSpeed = 1.0f;

    private float angle = 0.0f;
    private float timeSinceLastSpawn = 0.0f;

    private float minFireDelay = 0.01f;

    private Queue<Rigidbody2D> bulletPool = new Queue<Rigidbody2D>();
    private List<Rigidbody2D> activeBullets = new List<Rigidbody2D>();

    private void Awake() {
        for (int i = 0; i < startingPoolSize; i++)
            CreatePooledBullet();        
    }

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

        //check active bullets to see if they should be deactivated
        for (int i = activeBullets.Count - 1; i >= 0; i--) {
            float dist = (transform.position - activeBullets[i].transform.position).magnitude;
            if (dist >= deactivateBulletDistance) {
                activeBullets[i].gameObject.SetActive(false);
                bulletPool.Enqueue(activeBullets[i]);
                activeBullets.RemoveAt(i);
            }
        }
    }

    private void SpawnProjectile(float angle, float age)
    {
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 offset = dir * projectileSpeed * age;

        /*GameObject projectile = Instantiate(
            prefab,
            transform.position + (Vector3)offset,
            Quaternion.identity
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = dir * projectileSpeed;*/

        //activate bullet from pool
        Rigidbody2D projectile = bulletPool.Dequeue();
        projectile.transform.position = transform.position + (Vector3)offset;
        //projectile.transform.rotation = Quaternion.LookRotation(dir); //don't need this line for now, but if we add oblong or asymmetric sprites, this should rotate them in the dorrect direction
        projectile.gameObject.SetActive(true);
        projectile.velocity = dir.normalized * projectileSpeed;

        activeBullets.Add(projectile);
    }

    private void CreatePooledBullet() {
        Rigidbody2D b = Instantiate(prefab).GetComponent<Rigidbody2D>();
        bulletPool.Enqueue(b);
        b.gameObject.SetActive(false);
    }

}
