using Com.AsterForge.ShurikenRush.System.Core.Signal;

namespace Com.AsterForge.ShurikenRush.System.Core.DI
{
    public class GameContextReadySignal : BaseSignal
    {
        public GameContextReadySignal(bool isDebugMessage=true) : base(isDebugMessage) {}
        public override string ToString() => $"<GameContextReadySignal>";
    }
        
    
}