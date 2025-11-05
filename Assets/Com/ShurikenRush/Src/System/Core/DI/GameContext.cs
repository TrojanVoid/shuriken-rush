using System.Collections.Generic;
using Com.ShurikenRush.Manager.EnemyManager;
using Com.ShurikenRush.Manager.ProjectileManager;
using Com.ShurikenRush.World.Entity.Player;
using Com.ShurikenRush.Manager.ShurikenManager;
using Com.ShurikenRush.System.Core.SignalBus;
using Com.ShurikenRush.World.Entity.Enemy;

namespace Com.ShurikenRush.System.DI
{
    public static class GlobalContext
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

            IsInitialized = true;
            SignalBus.FireSignal<>(new GameContextReadySignal());
        }
        
        public static void Reset()
        {
            ShurikenManager = null;
            ProjectileManager = null;
            EnemyManager = null;
            PlayerController = null;
            IsInitialized = false;
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