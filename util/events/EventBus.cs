using System;
using System.Collections.Generic;

namespace Game.util.events {
    public static class EventBus {
        private static readonly Dictionary<object, EventRegistry> registries = [];

        public delegate void EventTriggeredAction<E>(E e) where E : EventArgs;

        public static void Subscribe<E>(
            this object src, EventTriggeredAction<E> action
        ) where E : EventArgs {
            if (!EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                registry = new EventRegistry();
                EventBus.registries[src] = registry;
            } 
            Type t = typeof(E);
            EventChannel<E>.OnEvent -= action;
            EventChannel<E>.OnEvent += action;
            registry.Unregisterers[t] = () => EventChannel<E>.OnEvent -= action;
            registry.Registerers[t] = () => EventChannel<E>.OnEvent += action;
        }

        public static void Mute<E>(this object src) where E : EventArgs {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                Type t = typeof(E);
                if (registry.Unregisterers.TryGetValue(t, out Action unregister) 
                        && registry.Muted.Add(t)) {
                    unregister();
                }
            }
        }

        public static void Reconnect<E>(this object src) where E : EventArgs {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                Type t = typeof(E);
                if (registry.Muted.Remove(t) 
                        && registry.Registerers.TryGetValue(t, out Action register)) {
                    register();
                }
            }
        }

        public static void MuteAll(this object src) {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                foreach (KeyValuePair<Type, Action> pair in registry.Unregisterers) {
                    if (registry.Muted.Add(pair.Key)) {
                        pair.Value();
                    }
                }
            }
        }

        public static void ReconnectAll(this object src) {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                foreach (Type t in registry.Muted) {
                    registry.Registerers[t]?.Invoke();
                }
            }
        }

        public static void Publish<E>(this object src, E e) where E : EventArgs {
            EventChannel<E>.OnEvent(e);
        }

        public static void ClearEvents(this object src) {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                foreach (KeyValuePair<Type, Action> pair in registry.Unregisterers) {
                    if (!registry.Muted.Contains(pair.Key)) {
                        pair.Value();
                    }
                }
                registry.Unregisterers.Clear();
                registry.Registerers.Clear();
                registry.Muted.Clear();
            }
        }

        /// <summary>
        /// Encapsulates a global event channel handling events of type <typeparamref name="E"/>.
        /// </summary>
        /// <typeparam name="E">The type of events handled by the channel.</typeparam>
        private static class EventChannel<E> where E : EventArgs {
            public static EventTriggeredAction<E> OnEvent { set; get; } = _ => {};
        }

        /// <summary>
        /// Encapsulates the event registry of a single object. Each event type should have at most one handler.
        /// </summary>
        private sealed class EventRegistry {
            public Dictionary<Type, Action> Unregisterers { get; } = [];
            public Dictionary<Type, Action> Registerers { get; } = [];
            public HashSet<Type> Muted { get; } = [];
        }
    }
}