using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool {
    private Queue<Rigidbody2D> pool = new Queue<Rigidbody2D>();
    public List<Rigidbody2D> active = new List<Rigidbody2D>();

    public void Init(MonoBehaviour parent, GameObject prefab, int startingSize) {
        for (int i = 0; i < startingSize; i++) {
            GameObject bullet = MonoBehaviour.Instantiate(prefab, parent.transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            rb.gameObject.SetActive(false);
            pool.Enqueue(rb);
        }
    }

    // Return the index of the spawned projectile
    public int Spawn() {
        if (pool.Count == 0) {
            return -1;
        }

        Rigidbody2D rb = pool.Dequeue();
        rb.gameObject.SetActive(true);
        active.Add(rb);
        return active.Count - 1;
    }

    public void Despawn(int index) {
        active[index].gameObject.SetActive(false);
        pool.Enqueue(active[index]);
        active.RemoveAt(index);
    }
}
