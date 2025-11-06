using System.Collections;
using System.Collections.Generic;
using Com.AsterForge.ShurikenRush.Systems.Core.DI.Bootstrapper;
using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.UserInterface.State;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.AsterForge.ShurikenRush.Systems.Core.Scene
{
    public enum SceneType { MainMenu, Game, Loading }
    
    public class SceneManager : MonoBehaviour
    {
        [Header("Scene Names")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        [SerializeField] private string loadingSceneName = "LoadingScene";

        private string _currentSceneName;
        private string _targetSceneName;
        private AsyncOperation _loadingOperation;
        private bool _isTransitioning;
        
    
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void LoadMainMenu()
        {
            LoadScene(mainMenuSceneName);
            GameState.CurrentState = GameStateType.MainMenu;
        }

        public void LoadLevel(string sceneName)
        {
            LoadScene(sceneName);
            GameState.CurrentState = GameStateType.Ingame;
        }

        private void LoadScene(string sceneName)
        {
            if (_isTransitioning || string.IsNullOrEmpty(sceneName)) return;
            if (sceneName == _currentSceneName) return;

            _isTransitioning = true;
            StartCoroutine(PerformSceneTransition(sceneName));
        }
        
        public SceneType GetCurrentSceneType()
        {
            var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (activeScene.Contains("MainMenu")) return SceneType.MainMenu;
            if (activeScene.Contains("Loading")) return SceneType.Loading;
            return SceneType.Game;
        }

        private IEnumerator PerformSceneTransition(string targetScene)
        {
            _targetSceneName = targetScene;

            if (GameContext.IsInitialized)
                GameContext.Reset();

            // Load Loading Scene first if defined
            if (!string.IsNullOrEmpty(loadingSceneName))
            {
                _loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);
                yield return new WaitUntil(() => _loadingOperation.isDone);
                yield return StartCoroutine(OnLoadingSceneShown());
            }

            // Now load the target scene
            _loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_targetSceneName, LoadSceneMode.Single);
            yield return new WaitUntil(() => _loadingOperation.isDone);

            _currentSceneName = _targetSceneName;
            yield return StartCoroutine(InitializeSceneContext(_currentSceneName));

            _isTransitioning = false;
        }

        protected virtual IEnumerator<object> OnLoadingSceneShown()
        {
            yield return null; // Optionally add loading animation timing
        }

        private IEnumerator InitializeSceneContext(string sceneName)
        {
            var bootstrapper = FindObjectOfType<GameContextBootstrapper>();

            if (bootstrapper != null)
            {
                yield return new WaitUntil(() => GameContext.IsInitialized);
                yield return StartCoroutine(OnLevelSceneReady(sceneName));
            }
            else
            {
                yield return StartCoroutine(OnMenuSceneReady(sceneName));
            }
        }

        protected virtual IEnumerator OnLevelSceneReady(string sceneName)
        {
            yield break; // Override in a subclass or add event hooks for level init
        }

        protected virtual IEnumerator OnMenuSceneReady(string sceneName)
        {
            yield break; // Override or subscribe externally for main menu setup
        }

        public void ReloadCurrentScene()
        {
            if (string.IsNullOrEmpty(_currentSceneName)) return;
            LoadScene(_currentSceneName);
        }
    }
}
