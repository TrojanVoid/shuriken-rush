using System;
using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.UserInterface.Signal;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Systems.Core.Pause
{
    public static class PauseManager
    {
        private static bool _isPaused;

        public static bool IsPaused => _isPaused;

        private static bool _initialized;

        public static void Initialize()
        {
            if (!_initialized)
            {
                SignalBus.Subscribe<PauseMenuTriggeredSignal>(OnPause);
            }
        }

        public static void Deinitialize()
        {
            if (_initialized)
            {
                SignalBus.Unsubscribe<PauseMenuTriggeredSignal>(OnPause);
            }
        }

        public static void PauseGame()
        {
            if (_isPaused) return;
            _isPaused = true;
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        public static void ResumeGame()
        {
            if (!_isPaused) return;
            _isPaused = false;
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }

        public static void TogglePause()
        {
            if (_isPaused) ResumeGame();
            else PauseGame();
        }

        private static void OnPause(PauseMenuTriggeredSignal signal)
        {   
            TogglePause();
        }
    }
}