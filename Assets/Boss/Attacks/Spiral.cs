using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : Attack {

    [Header("Attack Settings")]
    [SerializeField] float fireDelay = 1.0f;
    [Tooltip("Rate of turning in radians per second")]
    [SerializeField] float turnRate = Mathf.PI / 4.0f;
    [SerializeField] float projectileSpeed = 3.0f;
    [Tooltip("Initial angle in radians")]
    [SerializeField] float initialAngle = 0.0f;

    private float minFireDelay = 0.01f;

    private bool active = false;
    private float angle = 0.0f;
    private float timeSinceLastSpawn = 0.0f;

    public override void StartAttack() {
        active = true;
        angle = initialAngle;
        timeSinceLastSpawn = 0.0f;
    }

    public override void WhileAttacking() {
        if (!active) {
            return;
        }

        fireDelay = Mathf.Max(minFireDelay, fireDelay);
        timeSinceLastSpawn += Time.deltaTime;
        for (; timeSinceLastSpawn >= fireDelay; timeSinceLastSpawn -= fireDelay) {
            float timeOffset = timeSinceLastSpawn - fireDelay;
            float spawnAngle = angle - (turnRate * timeOffset);
            SpawnProjectile(spawnAngle, timeOffset, boss.transform.position);
        }
    }

    public override void StopAttack() {
        active = false;
    }

    private void SpawnProjectile(float angle, float age, Vector2 spawnPos) {
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 offset = dir * projectileSpeed * age;

        Projectile bullet = boss.BulletPool.Spawn(spawnPos + offset, dir * projectileSpeed);
        bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
    }
}

