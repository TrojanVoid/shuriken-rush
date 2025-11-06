using Com.AsterForge.ShurikenRush.Systems.Core.Observability;

namespace Com.AsterForge.ShurikenRush.UserInterface.Signal
{
    public class StartLevelSignal : BaseSignal
    {
        public int LevelIndex;

        public StartLevelSignal(int levelIndex = -1, bool isDebug = true)
        {
            LevelIndex = levelIndex;
            IsDebugMessage = isDebug;
        }

        public override string ToString() => $"<PlayButtonPressedSignal> Start Level for Level: {LevelIndex})";
    }

    public class PauseMenuTriggeredSignal : BaseSignal
    {
        public PauseMenuTriggeredSignal(bool isDebugMessage = true) : base(isDebugMessage) {}
        public override string ToString() =>  $"<PauseMenuTriggeredSignal> Pause Button Pressed.)";
    }
    
    public class OpenLevelSelectorSignal : BaseSignal
    {
        public OpenLevelSelectorSignal(bool isDebugMessage = true) : base(isDebugMessage) {}
        public override string ToString() =>  $"<LevelsButtonClickedSignal> Pause Button Pressed.)";
    }
    
    public class CloseLevelSelectorSignal : BaseSignal
    {
        public CloseLevelSelectorSignal(bool isDebugMessage = true) : base(isDebugMessage) {}
        public override string ToString() =>  $"<CloseLevelSelectorSignal> Pause Button Pressed.)";
    }
}