using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.Systems.Core.Scene;
using Com.AsterForge.ShurikenRush.Systems.Level.Data;
using Com.AsterForge.ShurikenRush.Systems.Level.Signal;
using Com.AsterForge.ShurikenRush.UserInterface.Signal;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Systems.Level.Service
{
    public class LevelService
    {
        public LevelService()
        {
            SignalBus.Subscribe<StartLevelSignal>(OnStartLevelSignal);
            SignalBus.Subscribe<LevelClearSignal>(OnLevelClear);
        }
        
        public string IndexToLevelName(int levelIndex) =>  $"Level_{levelIndex:D2}";


        private void OnStartLevelSignal(StartLevelSignal signal)
        {
            if (signal.LevelIndex == -1) // default value which means continue from last unlocked level
            {
                var currentLevel = LevelProgressData.GetHighestUnlockedLevel();
                GlobalContext.SceneManager.LoadLevel(IndexToLevelName(currentLevel));
            }
            else
            {
                GlobalContext.SceneManager.LoadLevel(IndexToLevelName(signal.LevelIndex));
            }
            
        }


        private void OnLevelClear(LevelClearSignal signal)
        {
            Debug.Log("[ SYSTEM : LEVEL SERVICE ] Level cleared.");
            var currentLevel = LevelProgressData.GetHighestUnlockedLevel();
            LevelProgressData.MarkLevelCompleted(currentLevel);
            if (currentLevel == LevelProgressData.GetMaxLevelCount())
            {
                Debug.Log("[ SYSTEM : LEVEL SERVICE ] Player has reached the last level. There is no more levels to play.");
            }
            else
            {
                Debug.Log("[ SYSTEM: LEVEL SERVICE ] Loading the next level...");
                GlobalContext.SceneManager.LoadLevel(IndexToLevelName(currentLevel + 1));
            }
            Debug.Log("current level: " + currentLevel);
            Debug.Log("max level:  " + LevelProgressData.GetMaxLevelCount());
        }
    }
}