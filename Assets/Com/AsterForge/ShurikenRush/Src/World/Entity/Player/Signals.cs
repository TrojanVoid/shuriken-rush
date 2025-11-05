using Com.AsterForge.ShurikenRush.System.Core.Signal;

namespace Com.AsterForge.ShurikenRush.World.Entity.Player
{
    public class PlayerHitSignal : BaseSignal
    {
        public int Damage;

        public PlayerHitSignal(int damage, bool isDebugMessage=true) : base(isDebugMessage)
        {
            Damage = damage;
        }
        
        public override string ToString() =>  $"<PlayerHitSignal> | Damage: ({Damage})";
    }
}