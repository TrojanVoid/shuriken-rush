using Com.AsterForge.ShurikenRush.Manager.Enemy;
using Com.AsterForge.ShurikenRush.Manager.Projectile;
using Com.AsterForge.ShurikenRush.Manager.Shuriken;
using Com.AsterForge.ShurikenRush.World.Entity.Player;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.System.Core.DI
{
    public class GameContextBootstrapper : MonoBehaviour
    {
        [SerializeField] private ShurikenManager shurikenManager;
        [SerializeField] private ProjectileManager projectileManager;
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private PlayerController playerController;

        private void Awake()
        {
            GameContext.Initialize(shurikenManager, projectileManager, enemyManager, playerController);
            DontDestroyOnLoad(gameObject);
        }
    }
}