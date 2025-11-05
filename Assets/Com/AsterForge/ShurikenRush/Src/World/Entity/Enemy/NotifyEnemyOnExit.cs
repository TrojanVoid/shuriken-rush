using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.Enemy
{
    public class NotifyEnemyOnExit : StateMachineBehaviour
    {
        public string methodName; // optional, to call different methods

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var enemy = animator.GetComponent<EnemyController>();
            if (enemy == null) return;

            if (!string.IsNullOrEmpty(methodName))
                enemy.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            else
                enemy.OnAnimationStateExit(stateInfo);
        }
    }
}