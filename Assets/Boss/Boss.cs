using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    public Attack[] attacks;

    private int currAttack = 0;
    private bool isAttacking = false;

    void Update()
    {
        if (!isAttacking && attacks.Length > 0)
        {
            isAttacking = true;
            StartCoroutine(Attack());
            isAttacking = false;
        }
    }

    private IEnumerator Attack()
    {
        Attack attack = attacks[currAttack];

        if (attack != null)
        {
            attack.attack.StartAttack();
            yield return new WaitForSeconds(attack.duration);
            attack.attack.StopAttack();
            yield return new WaitForSeconds(attack.cooldown);
        }

        currAttack = (currAttack + 1) % attacks.Length;
    }
}

[SerializeField]
public class Attack
{
    public float duration;
    public float cooldown;
    public AttackScript attack;
}

public abstract class AttackScript : MonoBehaviour
{
    public virtual void StartAttack() { }
    public virtual void StopAttack() { }
}

