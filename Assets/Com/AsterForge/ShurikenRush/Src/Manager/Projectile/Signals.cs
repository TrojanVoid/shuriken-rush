using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Manager.Projectile
{
    public struct ProjectileSpawnSignal
    {
        public Transform Origin;
        public Vector3 Direction;

        public ProjectileSpawnSignal(Transform origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}