using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Systems.Level.Data
{
    public static class LevelProgressData
    {
        private static LevelConfigSO _configSo;
        private static bool _initialized;
    
        private const string PREFS_KEY = "UnlockedLevel";

        private static int _unlockedLevel = -1;

        public static void Initialize(LevelConfigSO configSo)
        {
            if (_initialized) return;
            _configSo = configSo;
            _initialized = true;
            _unlockedLevel = PlayerPrefs.GetInt(PREFS_KEY, 1); // default: Level_01 unlocked
        }

        public static int GetHighestUnlockedLevel()
        {
            EnsureInitialized();
            return _unlockedLevel;
        }

        public static void MarkLevelCompleted(int levelNumber)
        {
            EnsureInitialized();

            if (levelNumber >= _unlockedLevel && levelNumber < _configSo.maxLevelCount)
            {
                _unlockedLevel = levelNumber + 1;
                PlayerPrefs.SetInt(PREFS_KEY, _unlockedLevel);
                PlayerPrefs.Save();
            }
        }

        public static bool IsLevelUnlocked(int levelNumber)
        {
            EnsureInitialized();
            return levelNumber <= _unlockedLevel;
        }

        public static int GetMaxLevelCount()
        {
            EnsureInitialized();
            return _configSo.maxLevelCount;
        }

        private static void EnsureInitialized()
        {
            if (!_initialized)
                Debug.LogError("LevelProgressData not initialized! You must call Initialize(LevelConfig) first.");
        }
    }
}