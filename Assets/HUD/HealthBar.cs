using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;

    float maxHealth;
    float currHealth;

    const float LERP_MULTIPLIER = 2f;

    public void Update() {
        float currValue = currHealth / maxHealth;
        if (currValue != healthBar.value) {
            float targetValue = Mathf.Lerp(healthBar.value, currValue, LERP_MULTIPLIER * Time.deltaTime);
            healthBar.value = targetValue;
        }
    }

    public void Init(int maxHealth) {
        this.maxHealth = maxHealth;
        currHealth = maxHealth;
        healthBar.value = 1;
    }

    public void TakeDamage(float damage) {
        currHealth -= damage;
        if (currHealth < 0) currHealth = 0;
    }
}
