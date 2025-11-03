using Com.ShurikenRush.World.Entity.Player;
using Com.ShurikenRush.World.Mass;

namespace Com.ShurikenRush.System.DIContainer
{
    public static class GlobalContextProvider
    {
        public static MassManager MassManager { get; internal set; }
        public static PlayerController PlayerController { get; internal set; }
        public static bool PlayerCanMove { get; internal set; }
    }
}