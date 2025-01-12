using UnityEngine;
public abstract class Attack : MonoBehaviour {
    public string ID;
    protected PrototypeBoss boss;
    public virtual void Init(PrototypeBoss boss) {
        this.boss = boss;
    }
    public abstract void StartAttack();
    public abstract void WhileAttacking();
    public abstract void StopAttack();
}
