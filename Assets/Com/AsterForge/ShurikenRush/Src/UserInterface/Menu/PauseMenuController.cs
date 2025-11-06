using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.UserInterface.Signal;
using Com.AsterForge.ShurikenRush.UserInterface.State;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.UserInterface.Menu
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private Canvas _canvas;

        private void Awake()
        {
            if (_canvas == null)
            {
                if (!TryGetComponent(out Canvas canvas))
                    throw new MissingComponentException(
                        "[ UI - MENU CONTROLLER ] No Canvas component found on the game object.");
                _canvas = canvas;
            }
            
        }

        private void OnEnable()
        {
            SignalBus.Subscribe<PauseMenuTriggeredSignal>(OnPauseMenuTriggered);
        }

        private void OnDisable()
        {
            SignalBus.Unsubscribe<PauseMenuTriggeredSignal>(OnPauseMenuTriggered);
        }

        private void OnPauseMenuTriggered(PauseMenuTriggeredSignal signal)
        {
            bool isEnabled = _canvas.enabled;
            _canvas.enabled = !isEnabled;
            if (!isEnabled) GameState.CurrentState = GameStateType.PauseMenu;

        }
    }
}