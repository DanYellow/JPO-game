using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    private Enemy enemy;
    public float cooldownDuration = 3.25f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(AnimationStrings.attack);
        enemy.ResetCanOperate(cooldownDuration);
        if (animator.TryGetComponent(out EnemyShoot enemyShoot))
        {
            enemyShoot.Shoot();
        }
    }
}
