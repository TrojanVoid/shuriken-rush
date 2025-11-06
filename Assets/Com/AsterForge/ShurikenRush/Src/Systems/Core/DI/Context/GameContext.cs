using Com.AsterForge.ShurikenRush.Manager.Enemy;
using Com.AsterForge.ShurikenRush.Manager.Projectile;
using Com.AsterForge.ShurikenRush.Manager.Shuriken;
using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.Systems.Core.Pause;
using Com.AsterForge.ShurikenRush.World.Entity.Player;

namespace Com.AsterForge.ShurikenRush.Systems.Core.DI.Context
{
    public static class GameContext
    {
        // DI CONTROL
        public static bool IsInitialized { get; private set; }
        
        public static void Initialize(
            ShurikenManager shurikenManager,
            ProjectileManager projectileManager,
            EnemyManager enemyManager,
            PlayerController playerController)
        {
            if (IsInitialized) return;

            ShurikenManager = shurikenManager;
            ProjectileManager = projectileManager;
            EnemyManager = enemyManager;
            PlayerController = playerController;

            PauseManager.Initialize();
            
            IsInitialized = true;
            SignalBus.FireSignal<GameContextReadySignal>(new GameContextReadySignal());
        }
        
        public static void Reset()
        {
            ShurikenManager = null;
            ProjectileManager = null;
            EnemyManager = null;
            PlayerController = null;
            IsInitialized = false;
            PauseManager.Deinitialize();
        }
        
        // MANAGER INSTANCES
        public static ShurikenManager ShurikenManager { get; internal set; }
        public static ProjectileManager ProjectileManager { get; internal set; }
        public static EnemyManager EnemyManager  { get; internal set; }
        
        // GAME OBJECT INSTANCES
        public static PlayerController PlayerController { get; internal set; }
        
        
        // PLAYER STATE
        private static bool _playerCanMoveHorizontal = false;
        public static bool PlayerCanMoveHorizontal
        {
            get => _playerCanMoveHorizontal;
            internal set
            {
                if (value == _playerCanMoveHorizontal) return;
                _playerCanMoveHorizontal = value;
                PlayerController.SetCanMoveHorizontal(value);
            }
        }

        private static bool _playerCanMoveVertical = true;
        public static bool PlayerCanMoveVertical
        {
            get => _playerCanMoveVertical;
            internal set
            {
                if (value == _playerCanMoveVertical) return;
                _playerCanMoveVertical = value;
                PlayerController.SetCanMoveVertical(value);
                    
            }
        }

        private static bool _massCanThrow = false;
        public static bool MassCanThrow
        {
            get => _massCanThrow;
            internal set
            {
                if (value == _massCanThrow) return;
                _massCanThrow = value;
                ShurikenManager.SetCanThrow(value);
            }
        }
    }
}