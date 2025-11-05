namespace Com.AsterForge.ShurikenRush.System.Core.Signal
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