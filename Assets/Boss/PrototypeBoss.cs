using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeBoss : MonoBehaviour
{
    [System.Serializable]
    private struct AttackState {
        [Tooltip("Must match trigger name for animation state!")] public string StateName;
        public float SpawnChance;
        public float Cooldown;
    }

    [SerializeField] Animator controller;
    [Header("Attack Settings")]
    [SerializeField] float attackIntervals;
    [SerializeField] AttackState[] attacks;

}
