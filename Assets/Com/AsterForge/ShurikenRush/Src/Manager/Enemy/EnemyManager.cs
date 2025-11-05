using System.Collections.Generic;
using System.Linq;
using Com.AsterForge.ShurikenRush.System.Core.DI;
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
            _enemies
                .FindAll(enemy => IsEnemyInRange(enemy.transform.position) && !enemy.IsAttacking && !enemy.IsDead)
                .ForEach(enemy =>
                {   
                    enemy.StartAttack();
                });
        }

        private bool IsEnemyInRange(Vector3 enemyPos)
        {
            float dist = (_player.transform.position - enemyPos).magnitude;
            return dist <= _enemyAttackRange;
        }

        private void ValidateComponents()
        {
            if (_enemiesParentRoot == null)
            {
                throw new MissingComponentException("[MANAGER : ENEMY MANAGER] Enemies Parent Root is missing.");
            }
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