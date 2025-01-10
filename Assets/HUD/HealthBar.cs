using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] TMP_Text labelText;
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

    public void Init(int maxHealth, string label) {
        labelText.SetText(label);
        this.maxHealth = maxHealth;
        currHealth = maxHealth;
        healthBar.value = 1;
    }

    public void TakeDamage(float damage) {
        currHealth -= damage;
        if (currHealth < 0) currHealth = 0;
    }
}
