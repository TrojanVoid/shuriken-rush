using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.Systems.Core.Scene;
using Com.AsterForge.ShurikenRush.Systems.Level.Data;
using Com.AsterForge.ShurikenRush.Systems.Level.Service;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Systems.Core.DI.Bootstrapper
{
    public class GlobalContextBootstrapper : MonoBehaviour
    {
        [Header("GlobalContext Wiring")]
        [Header("Managers")]
        [SerializeField] private SceneManager _sceneManagerPrefab;
     
        [Header("Global Configs")]
        [SerializeField] private LevelConfigSO _levelConfig;
        
        private void Awake()
        {
            if (!GlobalContext.IsInitialized)
            {
                // scene manager
                var existing = FindObjectOfType<SceneManager>();
                if (existing == null)
                    existing = Instantiate(_sceneManagerPrefab);

                // level service
                LevelService levelService = new LevelService();
                
                GlobalContext.Initialize(existing, levelService);
                DontDestroyOnLoad(existing.gameObject);
                LevelProgressData.Initialize(_levelConfig);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}