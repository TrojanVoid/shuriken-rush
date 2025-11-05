using Com.AsterForge.ShurikenRush.System.Core.Signal;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Manager.Projectile
{
    public class ProjectileSpawnSignal : BaseSignal
    {
        public Transform Origin;
        public Vector3 Direction;
        public int Damage;

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