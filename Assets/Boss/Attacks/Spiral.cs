using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Spiral : StateMachineBehaviour
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

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        angle = initialAngle;
        timeSinceLastSpawn = 0.0f;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        fireDelay = Mathf.Max(minFireDelay, fireDelay);
        timeSinceLastSpawn += Time.deltaTime;
        for (; timeSinceLastSpawn >= fireDelay; timeSinceLastSpawn -= fireDelay) {
            float timeOffset = timeSinceLastSpawn - fireDelay;
            float spawnAngle = angle - (turnRate * timeOffset);
            SpawnProjectile(spawnAngle, timeOffset, animator.transform.position);

        }

        angle += turnRate * Time.deltaTime;
        angle %= 2.0f * Mathf.PI;
    }

    private void SpawnProjectile(float angle, float age, Vector2 spawnPos)
    {
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 offset = dir * projectileSpeed * age;

        Rigidbody2D rb = Instantiate(prefab, spawnPos, Quaternion.identity).GetComponent<Rigidbody2D>();
        rb.transform.position = spawnPos + offset;
        rb.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        rb.velocity = dir * projectileSpeed;
    }
}
