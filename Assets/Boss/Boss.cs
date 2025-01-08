using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] AttackOld[] attacks;

    private int currAttack = 0;
    private bool isAttacking = false;

    void Update()
    {
        if (isAttacking)
        {
            return;
        }

        if (attacks.Length > 0)
        {
            isAttacking = true;
            StartCoroutine(Attack());
            isAttacking = false;
        }
    }

    private IEnumerator Attack()
    {
        AttackOld attack = attacks[currAttack];

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

/*[System.Serializable]*/
[System.Serializable]
public class AttackOld
{
    public float duration;
    public float cooldown;
    public AttackScript attack;
}

public abstract class AttackScript : MonoBehaviour
{
    protected bool isActive = false;

    public virtual void StartAttack()
    {
        isActive = true;
    }

    public virtual void StopAttack()
    {
        isActive = false;
    }
}
