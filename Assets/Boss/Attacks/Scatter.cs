using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatter : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] GameObject prefab;

    [Header("Attack Settings")]
    [SerializeField] int count = 5;
    [Tooltip("Attack angle in radians")]
    [SerializeField] float angle = 0.0f;
    [Tooltip("Projectile spread in radians")]
    [SerializeField] float spreadAngle = Mathf.PI / 6.0f;
    [SerializeField] float projectileSpeed = 3.0f;

    private bool active = true;

    void Update()
    {
        if (!active)
        {
            return;
        }

        float angleStep = count > 1 ? spreadAngle / (count - 1) : 0.0f;
        float startAngle = count > 1 ? angle - (spreadAngle / 2) : angle;

        for (int i = 0; i < count; i++)
        {
            float spawnAngle = startAngle + (angleStep * i);
            SpawnProjectile(spawnAngle);
        }

        active = false;
    }

    public void StartAttack()
    {
        active = true;
    }

    public void StopAttack()
    {
        active = false;
    }

    public bool IsAttacking()
    {
        return active;
    }

    private void SpawnProjectile(float angle)
    {
        GameObject projectile = Instantiate(prefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        /*rb.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);*/
        rb.velocity = dir * projectileSpeed;
    }
}
