using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatter : Attack
{
    [Header("Attack Settings")]
    [SerializeField] int count = 5;
    [Tooltip("Attack angle in radians")]
    [SerializeField] float angle = 0.0f;
    [Tooltip("Projectile spread in radians")]
    [SerializeField] float spreadAngle = Mathf.PI / 6.0f;
    [SerializeField] float projectileSpeed = 3.0f;

    public override void StartAttack() {
        float angleStep = count > 1 ? spreadAngle / (count - 1) : 0.0f;
        float startAngle = count > 1 ? angle - (spreadAngle / 2) : angle;

        for (int i = 0; i < count; i++) {
            float spawnAngle = startAngle + (angleStep * i);
            SpawnProjectile(spawnAngle);
        }
    }
    public override void WhileAttacking() { }
    public override void StopAttack() { }

    private void SpawnProjectile(float angle)
    {
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        /*rb.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);*/

        boss.BulletPool.Spawn(boss.transform.position, dir * projectileSpeed);
    }
}
