using System;

namespace Com.AsterForge.ShurikenRush.System.Animator
{
    public interface IAnimatable<in TState> where TState : Enum
    {
        void TriggerAnimationState(TState state);
        void Stop();
    }
}