using System.Collections.Generic;
using System.Linq;
using Com.AsterForge.ShurikenRush.System.Core.DI;
using Com.AsterForge.ShurikenRush.System.Core.Signal;
using Com.AsterForge.ShurikenRush.World.Entity.Enemy;
using Com.AsterForge.ShurikenRush.World.Entity.Player;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Manager.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform _enemiesParentRoot;
        [SerializeField, Range(1.0f, 20f)] private float _enemyAttackRange;

        private PlayerController _player;
        private List<EnemyController> _enemies;
        

        #region Unity Object Life Cycle
        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            _enemies = FindEnemiesInScene();
            if (_enemies == null || _enemies.Count == 0)
            {
                Debug.LogWarning("[ MANAGER : ENEMY MANAGER ]  Enemies are null or empty.");
            }
            Debug.Log("enemies count: " + _enemies.Count);
            
            if (GameContext.PlayerController == null)
            {
                throw new MissingComponentException(
                    "[ MANAGER : ENEMY MANAGER ] PlayerController component is missing in GlobalContext.");
            }
            else
            {
                _player = GameContext.PlayerController;
            }
        }

        private void Initialize()
        {
            ValidateComponents();
        }

        private void Update()
        {
            Tick();
        }

        public void Tick()
        {
            _enemies.RemoveAll(enemy => enemy.IsDead);
            if (!_player || !_enemies.Any(e => e != null)) return;

            _enemies.ForEach(enemy =>
            {
                if (!enemy.IsInitialized) return;
                bool inRange = IsEnemyInRange(enemy.transform.position); 
                if ( inRange && !enemy.IsAttacking)
                    enemy.StartAttack();
                else if( !inRange && enemy.IsAttacking)
                    enemy.StopAttack();
            });
        }

        private void OnEnable()
        {
            SignalBus.Subscribe<EnemyHitSignal>(OnEnemyHitSignal);
        }

        private void OnDisable()
        {
            SignalBus.Unsubscribe<EnemyHitSignal>(OnEnemyHitSignal);
        }

        private void OnEnemyHitSignal(EnemyHitSignal enemyHitSignal)
        {
            var enemy = enemyHitSignal.EnemyHit;
            enemy.TakeDamage();
        }

        private void ValidateComponents()
        {
            if (_enemiesParentRoot == null)
            {
                throw new MissingComponentException("[MANAGER : ENEMY MANAGER] Enemies Parent Root is missing.");
            }
        }
        
        private bool IsEnemyInRange(Vector3 enemyPos)
        {
            float dist = (_player.transform.position - enemyPos).magnitude;
            return dist <= _enemyAttackRange;
        }
        

        private List<EnemyController> FindEnemiesInScene()
        {
            return _enemiesParentRoot.GetComponentsInChildren<EnemyController>().ToList();
        }

        public void SetEnemyList(List<EnemyController> enemies)
        {
            _enemies = enemies;
        }
        #endregion
    }
}