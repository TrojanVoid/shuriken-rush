using System;

namespace Com.AsterForge.ShurikenRush.Systems.Animator
{
    public interface IAnimatable<in TState> where TState : Enum
    {
        void TriggerAnimationState(TState state);
        void Stop();
    }
}