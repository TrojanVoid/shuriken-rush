using Com.AsterForge.ShurikenRush.Manager.Enemy;
using Com.AsterForge.ShurikenRush.Manager.Projectile;
using Com.AsterForge.ShurikenRush.Manager.Shuriken;
using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.World.Entity.Player;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Systems.Core.DI.Bootstrapper
{
    public class GameContextBootstrapper : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private ShurikenManager _shurikenManager;
        [SerializeField] private ProjectileManager _projectileManager;
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private PlayerController _playerController;
        
        [Header("Debug Mode")]
        [SerializeField] private bool _debugMode;

        private void Awake()
        {
            GameContext.Initialize(_shurikenManager, _projectileManager, _enemyManager, _playerController);
            //DontDestroyOnLoad(gameObject);
            SignalBus.DebugMode = _debugMode;
        }
    }
}