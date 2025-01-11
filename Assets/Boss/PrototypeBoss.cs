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

    float attackTimer = 0;
    float cumulativeAttackSpawnChance = 0;
    int lastAttackMade;

    private void Awake() {
        ResetAttackTimer();
        for (int i = 0; i < attacks.Length; i++)
            cumulativeAttackSpawnChance += attacks[i].SpawnChance;
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

        Debug.LogWarning($"CAUTION: Could not get random attack state for {nameof(PrototypeBoss)}. Defaulting to last attack in inspector.");
        return attacks[attacks.Length - 1].StateName;
    }
}
