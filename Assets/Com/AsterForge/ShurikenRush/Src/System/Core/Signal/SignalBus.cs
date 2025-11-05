using System;
using System.Collections.Generic;

namespace Com.AsterForge.ShurikenRush.System.Core.Signal
{
    public static class SignalBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        public static void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();

            _subscribers[type].Add(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var list))
                list.Remove(callback);
        }

        public static void FireSignal<T>(T signal)
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var list))
            {
                foreach (var d in list.ToArray())
                    ((Action<T>)d)?.Invoke(signal);
            }
        }
    }

}