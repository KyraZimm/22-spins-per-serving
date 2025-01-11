using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool {
    private Queue<Projectile> pool = new Queue<Projectile>();
    private HashSet<Projectile> active = new HashSet<Projectile>();

    private GameObject prefab;
    private Transform poolRoot;

    public void Init(GameObject prefab, int startingSize) {
        //if any bullets exist for some reason (eg. uncleared Editor scene), destroy old pool
        ClearPool();

        poolRoot = new GameObject("Bullet Pool").transform;
        poolRoot.position = Vector3.zero;

        for (int i = 0; i < startingSize; i++)
            CreateNewPooledBullet(prefab);
    }

    private void CreateNewPooledBullet(GameObject prefab) {
        Projectile bullet = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, poolRoot).GetComponent<Projectile>();
        bullet.Init(this);
        pool.Enqueue(bullet);
    }

    private void ClearPool() {
        if (pool.Count > 0) {
            foreach (var bullet in pool)
                GameObject.Destroy(bullet.gameObject);
            pool.Clear();
        }
        if (active.Count > 0) {
            foreach (var bullet in active)
                GameObject.Destroy(bullet.gameObject);
            active.Clear();
        }
    }

    public Projectile Spawn(Vector2 spawnPos, Vector2 velocity) {
        if (pool.Count == 0) {
            CreateNewPooledBullet(prefab);
        }

        Projectile bullet = pool.Dequeue();
        bullet.gameObject.SetActive(true);
        bullet.Spawn(spawnPos, velocity);
        active.Add(bullet);
        return bullet;
    }

    public void Despawn(Projectile bullet) {
        if (!active.Contains(bullet)) return;

        bullet.gameObject.SetActive(false);
        pool.Enqueue(bullet);
        active.Remove(bullet);
    }
}
