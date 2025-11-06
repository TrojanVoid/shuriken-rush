using Com.AsterForge.ShurikenRush.Systems.Core.Scene;
using Com.AsterForge.ShurikenRush.Systems.Level.Service;

namespace Com.AsterForge.ShurikenRush.Systems.Core.DI.Context
{
    public static class GlobalContext
    {
        public static bool IsInitialized { get; private set; }
        public static SceneManager SceneManager { get; internal set; }
        public static LevelService LevelService { get; internal set; }

        public static void Initialize(SceneManager sceneManager, LevelService levelService)
        {
            if (IsInitialized) return;

            SceneManager = sceneManager;
            LevelService = levelService;
            IsInitialized = true;
        }
    }
}