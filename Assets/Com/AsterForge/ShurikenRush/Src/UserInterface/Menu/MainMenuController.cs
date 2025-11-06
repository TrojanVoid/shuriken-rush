using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.UserInterface.Signal;
using Com.AsterForge.ShurikenRush.UserInterface.State;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.UserInterface.Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private Canvas _mainMenuCanvas;

        [SerializeField] private Canvas _levelSelectorCanvas;

        private void Awake()
        {
            if (_mainMenuCanvas == null)
            {
                throw new MissingComponentException(
                    "[ USER INTERFACE - MAIN MENU CONTROLLER ] Main Menu Canvas component is not wired.");
            }

            if (_levelSelectorCanvas == null)
            {
                throw new MissingComponentException(
                    "[ USER INTERFACE - MAIN MENU CONTROLLER ]  Level Selector Canvas component is not wired.");
            }
        }

        private void OnEnable()
        {
            SignalBus.Subscribe<OpenLevelSelectorSignal>(OnOpenLevelSelectorSignal);
            SignalBus.Subscribe<CloseLevelSelectorSignal>(OnCloseLevelSelectorSignal);
        }

        private void OnDisable()
        {
            SignalBus.Unsubscribe<OpenLevelSelectorSignal>(OnOpenLevelSelectorSignal);
            SignalBus.Unsubscribe<CloseLevelSelectorSignal>(OnCloseLevelSelectorSignal);
        }

        private void OnCloseLevelSelectorSignal(CloseLevelSelectorSignal signal)
        {
            _mainMenuCanvas.enabled = true;
            _mainMenuCanvas.gameObject.SetActive(true);
            
            _levelSelectorCanvas.enabled = false;
            _levelSelectorCanvas.gameObject.SetActive(false);

            GameState.CurrentState = GameStateType.MainMenu;
        }

        private void OnOpenLevelSelectorSignal(OpenLevelSelectorSignal signal)
        {
            _mainMenuCanvas.enabled = false;
            _mainMenuCanvas.gameObject.SetActive(false);
            
            _levelSelectorCanvas.enabled = true;
            _levelSelectorCanvas.gameObject.SetActive(true);

            GameState.CurrentState = GameStateType.LevelSelector;
        }
        
        
    }
}