using Com.AsterForge.ShurikenRush.System.Core.Signal;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Manager.Projectile
{
    public class ProjectileSpawnSignal : BaseSignal
    {
        public readonly Transform Origin;
        public readonly Vector3 Direction;
        public readonly int Damage;

        public ProjectileSpawnSignal(Transform origin, Vector3 direction, int damage, bool isDebugMessage = true) 
            : base(isDebugMessage)
        {
            Origin = origin;
            Direction = direction;
            Damage = damage;
        }
        
        public override string ToString() => $"<ProjectileSpawnSignal> | Origin: {Origin}, Direction: {Direction}";
    }
}