using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Manager.Projectile
{
    public class ProjectileManager : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private GameObject _projectilePrefab;

        [SerializeField] private Transform _projectilesParent;
        [SerializeField, Range(1f, 30f)] private float _projectileSpeed = 20f;

        private void Awake()
        {
            Validate();
        }

        private void Validate()
        {
            if (_projectilePrefab == null)
                throw new MissingComponentException(
                    "[ MANAGER: PROJECTILE MANAGER ] Projectile Prefab is not set.");
            if (_projectilesParent == null)
                throw new MissingComponentException(
                    "[ MANAGER: PROJECTILE MANAGER ] Projectiles Container Parent is not set.");
        }
        
        private void OnEnable()
        {
            SignalBus.Subscribe<ProjectileSpawnSignal>(OnProjectileSpawn);
        }

        private void OnDisable()
        {
            SignalBus.Unsubscribe<ProjectileSpawnSignal>(OnProjectileSpawn);
        }

        private void OnProjectileSpawn(ProjectileSpawnSignal signal)
        {
            var projectile = Instantiate(_projectilePrefab, signal.Origin.position, Quaternion.identity);
            //projectile.transform.SetParent(_projectilesParent);
            var rb = projectile.GetComponentInChildren<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = signal.Direction * _projectileSpeed;
        }
    }

}