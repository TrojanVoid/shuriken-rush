using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.Systems.Core.Scene;
using Com.AsterForge.ShurikenRush.Systems.Level.Signal;
using Com.AsterForge.ShurikenRush.UserInterface.Signal;

namespace Com.AsterForge.ShurikenRush.Systems.Level.Service
{
    public class LevelService
    {
        public LevelService()
        {
            SignalBus.Subscribe<PlayButtonPressedSignal>(OnPlayButtonPressed);
            SignalBus.Subscribe<LevelClearSignal>(OnLevelClear);
        }

        private void OnPlayButtonPressed(PlayButtonPressedSignal signal)
        {
            var currentLevel = LevelProgressData.GetUnlockedLevelCount();
            var levelName = $"Level_{currentLevel:D2}";
            GlobalContext.SceneManager.LoadLevel(levelName);
        }

        private void OnLevelClear(LevelClearSignal signal)
        {
            var currentLevel = LevelProgressData.GetUnlockedLevelCount();
            LevelProgressData.MarkLevelCompleted(currentLevel);
        }
    }
}