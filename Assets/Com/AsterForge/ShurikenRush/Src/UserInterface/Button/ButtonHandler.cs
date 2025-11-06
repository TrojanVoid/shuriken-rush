using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.Systems.Core.Scene;
using Com.AsterForge.ShurikenRush.UserInterface.Signal;
using TMPro;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.UserInterface.Button
{
    public class ButtonHandler : MonoBehaviour
    {
        private enum ButtonActionType { Play, Pause, Levels, Settings, Exit }

        [SerializeField] private UnityEngine.UI.Button button;
        [SerializeField] private ButtonActionType buttonActionType;

        private void Awake()
        {
            if (button == null) button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            switch (buttonActionType)
            {
                case ButtonActionType.Play:
                    HandlePlayButton();
                    break;
                
                case ButtonActionType.Pause:
                    HandlePauseButton();
                    break;

                case ButtonActionType.Levels:
                    Debug.Log("Levels button pressed (placeholder)");
                    break;

                case ButtonActionType.Settings:
                    Debug.Log("Settings button pressed (placeholder)");
                    break;

                case ButtonActionType.Exit:
                    HandleExitButton();
                    break;
            }
        }

        private void HandlePlayButton()
        {
            SceneType sceneType = GlobalContext.SceneManager.GetCurrentSceneType();
            switch (sceneType)
            {
                case SceneType.MainMenu:
                    SignalBus.FireSignal(new PlayButtonPressedSignal());
                    break;
                case SceneType.Game:
                    SignalBus.FireSignal(new PauseMenuTriggeredSignal());
                    break;
            }
        }

        private void HandlePauseButton()
        {
            SceneType sceneType = GlobalContext.SceneManager.GetCurrentSceneType();
            if (sceneType == SceneType.Game)
            {
                SignalBus.FireSignal(new PauseMenuTriggeredSignal());
            }
        }

        private void HandleExitButton()
        {
            var sceneType = GlobalContext.SceneManager.GetCurrentSceneType();

            switch (sceneType)
            {
                case SceneType.MainMenu:
                    Debug.Log("Exit button pressed in MainMenu — quitting game...");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;

                case SceneType.Game:
                    Debug.Log("Exit button pressed in Game — returning to Main Menu...");
                    SignalBus.FireSignal(new PauseMenuTriggeredSignal());
                    GlobalContext.SceneManager.LoadMainMenu();
                    
                    break;

                case SceneType.Loading:
                    Debug.Log("Exit button pressed in Loading Scene — ignored.");
                    break;
            }
        }
    }
}
