using Com.AsterForge.ShurikenRush.Systems.Core.Observability;

namespace Com.AsterForge.ShurikenRush.UserInterface.Signal
{
    public class PlayButtonPressedSignal : BaseSignal
    {

        public PlayButtonPressedSignal(bool isDebug = true)
        {
            IsDebugMessage = isDebug;
        }

        public override string ToString() => $"<PlayButtonPressedSignal> Play Button Pressed.)";
    }

    public class PauseMenuTriggeredSignal : BaseSignal
    {
        public PauseMenuTriggeredSignal(bool isDebugMessage = true) : base(isDebugMessage) {}
        public override string ToString() =>  $"<PauseMenuTriggeredSignal> Pause Button Pressed.)";
    }
}