namespace Com.AsterForge.ShurikenRush.Systems.Core.Observability
{
    public abstract class BaseSignal
    {
        public bool IsDebugMessage;

        public BaseSignal(bool isDebugMessage = true)
        {
            IsDebugMessage = isDebugMessage;
        }

        public abstract override string ToString();
    }
}