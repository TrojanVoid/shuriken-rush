using Com.AsterForge.ShurikenRush.System.Core.Signal;
using Com.AsterForge.ShurikenRush.World.Entity.Player;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.Enemy
{
    public class EnemyColliderConnector : MonoBehaviour
    {
        private int _damage;
        private EnemyController _enemyController;
        
        private void Awake()
        {
            if (gameObject.transform.parent == null)
            {
                throw new MissingComponentException("[ ENTITY : ENEMY COLLIDER CONNECTOR ] No parent GameObject found.");
            }

            if (!gameObject.transform.parent.TryGetComponent<EnemyController>(out var enemyController))
            {
                throw new MissingComponentException("[ ENTITY : ENEMY COLLIDER CONNECTOR ] No EnemyController component found in parent Game Object.");
            }

            _enemyController = enemyController;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                PlayerHitSignal signal = new PlayerHitSignal(_damage);
                SignalBus.FireSignal<PlayerHitSignal>(signal);
            }

            if (other.gameObject.CompareTag("Shuriken"))
            {
                SignalBus.FireSignal<EnemyHitSignal>(new EnemyHitSignal(_enemyController));
            }
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }
    }
}