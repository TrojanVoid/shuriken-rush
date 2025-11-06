using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.Systems.Core.Scene;
using Com.AsterForge.ShurikenRush.UserInterface.Signal;
using Com.AsterForge.ShurikenRush.UserInterface.State;
using TMPro;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.UserInterface.Button
{
    public class ButtonHandler : MonoBehaviour
    {
        private enum ButtonActionType { Play, Pause, Levels, SelectLevel, Settings, Exit }

        [Header("Wiring")]
        [SerializeField] private UnityEngine.UI.Button button;
        [SerializeField] private ButtonActionType buttonActionType;
        
        [Header("(Optional) Value to use in the Button Handler function")]
        [SerializeField] private int _actionValue = -1;

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
                    HandlePlayAction();
                    break;
                
                case ButtonActionType.Pause:
                    HandlePauseAction();
                    break;

                case ButtonActionType.Levels:
                    HandleLevelsAction();
                    break;
                
                case ButtonActionType.SelectLevel:
                    HandleSelectLevelAction();
                    break;

                case ButtonActionType.Settings:
                    Debug.Log("Settings button pressed (placeholder)");
                    break;

                case ButtonActionType.Exit:
                    HandleExitAction();
                    break;
            }
        }

        private void HandlePlayAction()
        {
            SceneType sceneType = GlobalContext.SceneManager.GetCurrentSceneType();
            switch (sceneType)
            {
                case SceneType.MainMenu:
                    SignalBus.FireSignal(new StartLevelSignal());
                    break;
                case SceneType.Game:
                    SignalBus.FireSignal(new PauseMenuTriggeredSignal());
                    break;
            }
        }

        private void HandlePauseAction()
        {
            SceneType sceneType = GlobalContext.SceneManager.GetCurrentSceneType();
            if (sceneType == SceneType.Game)
            {
                SignalBus.FireSignal(new PauseMenuTriggeredSignal());
            }
        }

        private void HandleLevelsAction()
        {
            SignalBus.FireSignal(new OpenLevelSelectorSignal());
        }

        private void HandleSelectLevelAction()
        {
            if (GameState.CurrentState != GameStateType.LevelSelector) return;
            if (_actionValue < 1)
            {
                Debug.LogError($"[ USER INTERFACE : BUTTON HANDLER ] Cannot start a level with action value: {_actionValue}");
                return;
            }
            StartLevelSignal signal = new StartLevelSignal(_actionValue);
            SignalBus.FireSignal(signal);
        }

        private void HandleExitAction()
        {
            var gameState = GameState.CurrentState;
            //TODO Delete this
            Debug.Log("Current game state: " + gameState);

            switch (gameState)
            {
                case GameStateType.MainMenu:
                    Debug.Log("Exit button pressed in MainMenu — quitting game...");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;

                case GameStateType.PauseMenu:
                    Debug.Log("Exit button pressed in Game — returning to Main Menu...");
                    SignalBus.FireSignal(new PauseMenuTriggeredSignal());
                    GlobalContext.SceneManager.LoadMainMenu();
                    
                    break;
                
                case GameStateType.LevelSelector:
                    Debug.Log("Exit button pressed in Level Selector — returning to Main Menu...");
                    SignalBus.FireSignal(new CloseLevelSelectorSignal());
                    break;

                case GameStateType.Loading:
                    Debug.Log("Exit button pressed in Loading Scene — ignored.");
                    break;
            }
        }
    }
}
