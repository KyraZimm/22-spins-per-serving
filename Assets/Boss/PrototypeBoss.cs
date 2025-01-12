using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeBoss : MonoBehaviour
{
    [System.Serializable]
    private struct AttackState {
        [Tooltip("Must match trigger name for animation state!")] public string StateName;
        public float SpawnChance;
    }

    [SerializeField] Animator controller;
    [Header("Attack Settings")]
    [SerializeField] Vector2 attackIntervalRange;
    [SerializeField] AttackState[] attacks;
    [Header("Projectile Pool Settings")]
    [SerializeField] int startingPoolSize;
    [SerializeField] GameObject projectilePrefab;
    [Header("Health")]
    [SerializeField] int maxHealth;
    [SerializeField] HealthBar healthBar;

    public BulletPool BulletPool { get; private set; }

    float attackTimer = 0;
    float cumulativeAttackSpawnChance = 0;
    int lastAttackMade;

    public float CurrHealth { get; private set; }

    private void Awake() {
        ResetAttackTimer();
        for (int i = 0; i < attacks.Length; i++)
            cumulativeAttackSpawnChance += attacks[i].SpawnChance;

        BulletPool = new BulletPool();
        BulletPool.Init(projectilePrefab, startingPoolSize);

        CurrHealth = maxHealth;
        if (healthBar != null) healthBar.Init(maxHealth, "Prototype Boss");
    }

    private void Update() {
        AnimatorStateInfo animState = controller.GetCurrentAnimatorStateInfo(0);
        if (animState.IsName("Idle_Prototype")) {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) {
                controller.SetTrigger(GetRandomAttack());
                ResetAttackTimer(); //should theoretically get reset when the anim state ends instead, but I'll do it laterrrrr
            }
        }
    }

    private void ResetAttackTimer() {
        attackTimer = Random.Range(attackIntervalRange.x, attackIntervalRange.y);
    }

    private string GetRandomAttack() {
        float r = Random.Range(0, cumulativeAttackSpawnChance);
        float tracker = 0;
        for (int i = 0; i < attacks.Length; i++) {
            tracker += attacks[i].SpawnChance;
            if (tracker >= r) return attacks[i].StateName;
        }

        Debug.LogWarning($"CAUTION: Could not get random attack state for {nameof(PrototypeBoss)}. Defaulting to last attack in array.");
        return attacks[attacks.Length - 1].StateName;
    }

    public void TakeDamage(float damage) {
        CurrHealth -= damage;
        if (healthBar != null) healthBar.TakeDamage(damage);

        if (CurrHealth <= 0) Debug.Log("Boss is dead!");
    }
}
