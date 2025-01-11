using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralAttack : MonoBehaviour, BossAttack
{
    [Header("Projectile")]
    [SerializeField] GameObject prefab;

    [Header("Attack Settings")]
    [SerializeField] float fireDelay = 1.0f;
    [Tooltip("Rate of turning in radians per second")]
    [SerializeField] float turnRate = Mathf.PI / 4.0f;
    [SerializeField] float projectileSpeed = 3.0f;
    [Tooltip("Initial angle in radians")]
    [SerializeField] float initialAngle = 0.0f;

    private BulletPool bulletPool;
    private float minFireDelay = 0.01f;

    private bool active = false;
    private float angle = 0.0f;
    private float timeSinceLastSpawn = 0.0f;

    void Awake()
    {
        bulletPool = new BulletPool();
        bulletPool.Init(this, prefab, 100);
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

        fireDelay = Mathf.Max(minFireDelay, fireDelay);
        timeSinceLastSpawn += Time.deltaTime;
        for (; timeSinceLastSpawn >= fireDelay; timeSinceLastSpawn -= fireDelay)
        {
            float timeOffset = timeSinceLastSpawn - fireDelay;
            float spawnAngle = angle - (turnRate * timeOffset);
            SpawnProjectile(spawnAngle, timeOffset);

        }

        angle += turnRate * Time.deltaTime;
        angle %= 2.0f * Mathf.PI;

        DespawnProjectiles();
    }

    public void StartAttack()
    {
        active = true;
        angle = initialAngle;
        timeSinceLastSpawn = 0.0f;
    }

    public void StopAttack()
    {
        active = false;
    }

    public bool IsAttacking()
    {
        return active;
    }

    private void SpawnProjectile(float angle, float age)
    {
        int idx = bulletPool.Spawn();
        if (idx < 0)
        {
            return;
        }

        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 offset = dir * projectileSpeed * age;

        Rigidbody2D rb = bulletPool.active[idx].GetComponent<Rigidbody2D>();
        rb.transform.position = transform.position + (Vector3)offset;
        rb.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        rb.velocity = dir * projectileSpeed;
    }

    // Remove bullets that are too far away
    private void DespawnProjectiles()
    {
        for (int i = bulletPool.active.Count - 1; i >= 0; i--)
        {
            float dist = (transform.position - bulletPool.active[i].transform.position).magnitude;
            if (dist >= 10.0f)
            {
                bulletPool.Despawn(i);
            }
        }
    }
}

// A fixed size bullet pool
public class BulletPool
{
    private Queue<Rigidbody2D> pool = new Queue<Rigidbody2D>();
    public List<Rigidbody2D> active = new List<Rigidbody2D>();

    public void Init(MonoBehaviour parent, GameObject prefab, int startingSize)
    {
        for (int i = 0; i < startingSize; i++)
        {
            GameObject bullet = MonoBehaviour.Instantiate(prefab, parent.transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            rb.gameObject.SetActive(false);
            pool.Enqueue(rb);
        }
    }

    // Return the index of the spawned projectile
    public int Spawn()
    {
        if (pool.Count == 0)
        {
            return -1;
        }

        Rigidbody2D rb = pool.Dequeue();
        rb.gameObject.SetActive(true);
        active.Add(rb);
        return active.Count - 1;
    }

    public void Despawn(int index)
    {
        active[index].gameObject.SetActive(false);
        pool.Enqueue(active[index]);
        active.RemoveAt(index);
    }
}
