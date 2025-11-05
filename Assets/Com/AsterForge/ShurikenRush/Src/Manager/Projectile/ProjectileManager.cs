using Com.AsterForge.ShurikenRush.System.Core.Signal;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Manager.Projectile
{
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 20f;

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
            var projectile = Instantiate(projectilePrefab, signal.Origin.position, Quaternion.identity);
            var rb = projectile.GetComponentInChildren<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = signal.Direction * projectileSpeed;
        }
    }

}