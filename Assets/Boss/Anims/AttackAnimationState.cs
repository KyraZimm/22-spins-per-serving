using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationState : StateMachineBehaviour
{
    [SerializeField] string attackID;
    [SerializeField] float duration;

    Boss boss;
    float timer;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (boss == null) {
            boss = animator.GetComponent<Boss>();
            if (boss == null) Debug.LogError($"CRITICAL: There is no {nameof(Boss)} class on the same GameObject as {animator.ToString()}. Add one or delete the Animator on {animator.gameObject.name}.");
        }

        boss.LaunchAttack(attackID);
        timer = duration;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        timer -= Time.deltaTime;
        if (timer <= 0) animator.SetTrigger("End Attack");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateExit(animator, stateInfo, layerIndex);
        boss.StopCurrAttack();
    }
}
