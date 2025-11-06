using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Systems.Core.Observability
{
    public static class SignalBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        private static bool _debugMode = false;
        public static bool DebugMode
        {
            get => _debugMode;
            set => _debugMode = value;
        }
    
        
        public static void Subscribe<T>(Action<T> callback) where T : BaseSignal
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();

            _subscribers[type].Add(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback) where T : BaseSignal
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var list))
                list.Remove(callback);
        }

        
        public static void FireSignal<T>(T signal) where T : BaseSignal
        { 
            // this._debugMode is global-scoped where signal.IsDebugMessage is single signal scoped.
            if (_debugMode && signal.IsDebugMessage)
            {
                Debug.Log("[ SIGNAL BUS : FIRING SIGNAL ] Signal: " + signal.ToString());
            }
            
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var list))
            {
                foreach (var d in list.ToArray())
                    ((Action<T>)d)?.Invoke(signal);
            }
        }
    }

}