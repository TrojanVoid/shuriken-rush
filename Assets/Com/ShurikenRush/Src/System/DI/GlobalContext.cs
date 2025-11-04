using Com.ShurikenRush.World.Entity.Player;
using Com.ShurikenRush.World.Mass;

namespace Com.ShurikenRush.System.DI
{
    public static class GlobalContext
    {
        public static MassManager MassManager { get; internal set; }
        public static PlayerController PlayerController { get; internal set; }

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
                MassManager.SetCanThrow(value);
            }
        }
    }
}