using Com.AsterForge.ShurikenRush.Systems.Core.Observability;

namespace Com.AsterForge.ShurikenRush.Systems.Level.Signal
{
    public class LevelClearSignal : BaseSignal
    {
        public LevelClearSignal(bool isDebugMessage = true) : base(isDebugMessage) {}
        public override string ToString() =>  "<LevelClearSignal> Level Cleared.";
    }
}