using Com.AsterForge.ShurikenRush.Systems.Core.Observability;

namespace Com.AsterForge.ShurikenRush.World.Entity.Player
{
    public class PlayerHitSignal : BaseSignal
    {
        public readonly int Damage;

        public PlayerHitSignal(int damage, bool isDebugMessage=true) : base(isDebugMessage)
        {
            Damage = damage;
        }
        
        public override string ToString() =>  $"<PlayerHitSignal> | Damage: ({Damage})";
    }
}