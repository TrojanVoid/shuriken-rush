using Com.AsterForge.ShurikenRush.Systems.Core.Observability;

namespace Com.AsterForge.ShurikenRush.Systems.Core.DI
{
    public class GameContextReadySignal : BaseSignal
    {
        public GameContextReadySignal(bool isDebugMessage=true) : base(isDebugMessage) {}
        public override string ToString() => $"<GameContextReadySignal>";
    }
        
    
}