using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;

    int maxHealth;
    int currHealth;
    int targetHealth;

    public void Update() {
        if (currHealth != targetHealth) {
            float targetValue = targetHealth / maxHealth;
            float currValue = currHealth / maxHealth;

            float lerpedValue = Mathf.Lerp(currValue, targetValue, Time.deltaTime);
            healthBar.value = lerpedValue;
        }
    }

    public void Init(int maxHealth) {
        this.maxHealth = maxHealth;
        currHealth = maxHealth;
        targetHealth = maxHealth;
        healthBar.value = 1;
    }

    public void TakeDamage(int damage) {
        targetHealth = Mathf.Clamp(targetHealth - damage, 0, maxHealth);        
    }
}
