using UnityEngine;
public abstract class Attack : MonoBehaviour {
    public string ID;
    protected Boss boss;
    public virtual void Init(Boss boss) {
        this.boss = boss;
    }
    public abstract void StartAttack();
    public abstract void WhileAttacking();
    public abstract void StopAttack();
}
