using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [System.Serializable]
    private struct AttackState {
        public string AttackID;
        [Tooltip("Must match trigger name for animation state!")] public string AnimTriggerName;
        public float SpawnChance;
    }

    [SerializeField] Animator controller;
    [SerializeField] MonoBehaviour test;
    [Header("Attack Settings")]
    [SerializeField] Transform target;
    [SerializeField] Vector2 attackIntervalRange;
    [SerializeField] AttackState[] attacks;
    [SerializeField] Attack[] attachedAttackScripts;
    [Header("Projectile Pool Settings")]
    [SerializeField] int startingPoolSize;
    [SerializeField] GameObject projectilePrefab;
    [Header("Health")]
    [SerializeField] int maxHealth;
    [SerializeField] HealthBar healthBar;

    public BulletPool BulletPool { get; private set; }
    public Transform Target { get { return target; } }
    public float CurrHealth { get; private set; }

    float attackTimer = 0;
    float cumulativeAttackSpawnChance = 0;

    int currAttackIndex = -1;
    string currAttackID = string.Empty;
    Dictionary<string, Attack> attackMapper = new Dictionary<string, Attack>();


    private void Awake() {
        foreach (Attack script in attachedAttackScripts) {
            script.Init(this);
            attackMapper.Add(script.ID, script);
        }

        ResetAttackTimer();
        for (int i = 0; i < attacks.Length; i++)
            cumulativeAttackSpawnChance += attacks[i].SpawnChance;

        BulletPool = new BulletPool();
        BulletPool.Init(projectilePrefab, startingPoolSize);

        CurrHealth = maxHealth;
        if (healthBar != null) healthBar.Init(maxHealth, "Prototype Boss");
    }

    private void Update() {
        if (currAttackIndex < 0) {
            AnimatorStateInfo animState = controller.GetCurrentAnimatorStateInfo(0);
            if (animState.IsName("Idle_Prototype")) {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0) {
                    SetNewAttackState(GetRandomAttackIndex());
                }
            }
        }
        else {
            attackMapper[currAttackID].WhileAttacking();
        }
    }


    private void ResetAttackTimer() {
        attackTimer = Random.Range(attackIntervalRange.x, attackIntervalRange.y);
    }

    private int GetRandomAttackIndex() {
        float r = Random.Range(0, cumulativeAttackSpawnChance);
        float tracker = 0;
        for (int i = 0; i < attacks.Length; i++) {
            tracker += attacks[i].SpawnChance;
            if (tracker >= r) return i;
        }

        Debug.LogWarning($"CAUTION: Could not get random attack state for {nameof(Boss)} on {gameObject.name}.");
        return -1;
    }

    private void SetNewAttackState(int index) {
        currAttackIndex = index;
        currAttackID = attacks[index].AttackID;

        controller.SetTrigger(attacks[index].AnimTriggerName);
    }

    #region Extern
    public void TakeDamage(float damage) {
        CurrHealth -= damage;
        if (healthBar != null) healthBar.TakeDamage(damage);

        if (CurrHealth <= 0) Debug.Log("Boss is dead!");
    }

    public void LaunchAttack(string attackID) {
        attackMapper[currAttackID].StartAttack();
    }

    public void StopCurrAttack() {
        attackMapper[currAttackID].StopAttack();
        ResetAttackTimer();

        currAttackID = string.Empty;
        currAttackIndex = -1;
    }
    #endregion
}
