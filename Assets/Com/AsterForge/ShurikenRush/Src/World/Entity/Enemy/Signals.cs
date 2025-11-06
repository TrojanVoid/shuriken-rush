using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using NotImplementedException = System.NotImplementedException;

namespace Com.AsterForge.ShurikenRush.World.Entity.Enemy
{
    public class EnemyHitSignal : BaseSignal
    {
        public readonly EnemyController EnemyHit;
        public EnemyHitSignal(EnemyController enemyHit, bool isDebugMode = true) : base(isDebugMode)
        {
            EnemyHit = enemyHit;
        }

        public override string ToString() => $"<EnemyHitSignal> | EnemyHit: {EnemyHit}";
    }
}