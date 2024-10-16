using System;
using System.Collections.Generic;
using Game.util.math;

namespace Game.util.events {
    public static class EventBus {
        private static readonly Dictionary<object, EventRegistry> registries = [];

        /// <summary>
        /// A delegate type for event triggered actions.
        /// </summary>
        /// <typeparam name="E">The event type.</typeparam>
        /// <param name="sender">The event emitter.</param>
        /// <param name="e">The event.</param>
        private delegate void EventTriggeredAction<E>(object sender, Event<E> e) where E : EventArgs;

        /// <summary>
        /// Subscribe to events of type <typeparamref name="E"/> from <paramref name="src"/> with the given <paramref name="action"/> as the handler.
        /// </summary>
        /// <param name="src">The object subscribing to the events.</param>
        /// <param name="action">The event handler.</param>
        /// <typeparam name="E">The event type.</typeparam>
        public static void Subscribe<E>(this object src, Action<E> action) where E : EventArgs {
            if (!EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                registry = new EventRegistry();
                EventBus.registries[src] = registry;
            } 
            Type t = typeof(E);
            void fire(object sender, Event<E> e) {
                if (e.CanAccept(sender, src)) {
                    action(e.GameEvent);
                }
            }
            EventChannel<E>.OnEvent -= fire;
            EventChannel<E>.OnEvent += fire;
            registry.Unregisterers[t] = () => EventChannel<E>.OnEvent -= fire;
            registry.Registerers[t] = () => EventChannel<E>.OnEvent += fire;
        }

        /// <summary>
        /// Mute events of type <typeparamref name="E"/> from <paramref name="src"/>, which will pause the event handling for this event type.
        /// </summary>
        /// <param name="src">The object listening to the events.</param>
        /// <typeparam name="E">The event type.</typeparam>
        public static void Mute<E>(this object src) where E : EventArgs {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                Type t = typeof(E);
                if (registry.Unregisterers.TryGetValue(t, out Action unregister) 
                        && registry.Muted.Add(t)) {
                    unregister();
                }
            }
        }

        /// <summary>
        /// Resume listening to events of type <typeparamref name="E"/> from <paramref name="src"/>, which will resume the event handling for this event type.
        /// </summary>
        /// <param name="src">The object listening to the events.</param>
        /// <typeparam name="E">The event type.</typeparam>
        public static void Reconnect<E>(this object src) where E : EventArgs {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                Type t = typeof(E);
                if (registry.Muted.Remove(t) 
                        && registry.Registerers.TryGetValue(t, out Action register)) {
                    register();
                }
            }
        }

        /// <summary>
        /// Cause all events to be ignored by <paramref name="src"/>.
        /// </summary>
        /// <param name="src">An object.</param>
        public static void MuteAll(this object src) {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                foreach (KeyValuePair<Type, Action> pair in registry.Unregisterers) {
                    if (registry.Muted.Add(pair.Key)) {
                        pair.Value();
                    }
                }
            }
        }

        /// <summary>
        /// Resume all event subscriptions for <paramref name="src"/>.
        /// </summary>
        /// <param name="src">An object.</param>
        public static void ReconnectAll(this object src) {
            if (EventBus.registries.TryGetValue(src, out EventRegistry registry)) {
                foreach (Type t in registry.Muted) {
                    registry.Registerers[t]?.Invoke();
                }
            }
        }

        /// <summary>
        /// Publish an event <paramref name="e"/> of type <typeparamref name="E"/> to the specified <paramref name="targets"/> fulfilling the given <paramref name="condition"/>, via the channel for <typeparamref name="C"/>.
        /// </summary>
        /// <param name="src">The event emitter.</param>
        /// <param name="e">The event to be published.</param>
        /// <param name="targets">The valid targets for the event.</param>
        /// <param name="condition">An boolean condition on the event emitter and the target objects for the event to be accepted.</param>
        /// <typeparam name="E">The event type.</typeparam>
        public static void Publish<E>(
            this object src, E e, HashSet<object> targets = null, 
            Predicate<object, object> condition = null
        ) where E : EventArgs {
            EventChannel<E>.OnEvent(src, Event<E>.Of(e, targets, condition));
        }

        /// <summary>
        /// Permanently unsubscribe all events from <paramref name="src"/>. This method is invoked automatically when <paramref name="src"/> is recycled. Usually, we should not need to call this method directly.
        /// </summary>
        /// <param name="src">An object.</param>
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
            public static EventTriggeredAction<E> OnEvent { set; get; } = (_, _) => {};
        }

        /// <summary>
        /// Encapsulates the event registry of a single object. Each event type should have at most one handler.
        /// </summary>
        private sealed class EventRegistry {
            public Dictionary<Type, Action> Unregisterers { get; } = [];
            public Dictionary<Type, Action> Registerers { get; } = [];
            public HashSet<Type> Muted { get; } = [];
        }

        /// <summary>
        /// Wrapper class for a C# event used to transmit data between game objects.
        /// </summary>
        /// <typeparam name="E">The type of the event.</typeparam>
        private sealed class Event<E> where E : EventArgs {
            [Flags]
            private enum EventAttribute {
                None = 0,
                Directed = 1,
                Conditional = 2,
            }

            /// <summary>
            /// The actual event data.
            /// </summary>
            /// <value>An instance of <typeparamref name="E"/> which is a subtype o EventArgs.</value>
            public E GameEvent { get; private init; }
            private EventAttribute Attribute { get; init; }
            private HashSet<object> ValidTargets { get; init; }
            private Predicate<object, object> Condition { get; init; }

            private Event(
                E gameEvent, EventAttribute attribute, HashSet<object> validTargets,
                Predicate<object, object> Condition
            ) {
                this.GameEvent = gameEvent;
                this.Attribute = attribute;
                this.ValidTargets = validTargets;
                this.Condition = Condition;
            }

            private static Event<E> Of(E e, HashSet<object> targets) {
                if (targets == null || targets.Count == 0) {
                    return new Event<E>(e, EventAttribute.None, null, MathUtil.BINARY_TAUTOLOGY);
                }
                return new Event<E>(e, EventAttribute.Directed, targets, MathUtil.BINARY_TAUTOLOGY);
            }

            private static Event<E> Of(E e, Predicate<object, object> condition) {
                if (condition == null) {
                    return new Event<E>(e, EventAttribute.None, null, MathUtil.BINARY_TAUTOLOGY);
                }
                return new Event<E>(e, EventAttribute.Conditional, null, condition);
            }

            public static Event<E> Of(
                E e, HashSet<object> targets = null, Predicate<object, object> condition = null
            ) {
                if (targets == null || targets.Count == 0) {
                    return Event<E>.Of(e, condition);
                }
                if (condition == null) {
                    return Event<E>.Of(e, targets);
                }
                return new Event<E>(
                    e, EventAttribute.Directed | EventAttribute.Conditional, 
                    targets, condition
                );
            }

            /// <summary>
            /// Checks if the event from <paramref name="src"/>can be accepted by <paramref name="obj"/>.
            /// </summary>
            /// <param name="src">The emitter of the event.</param>
            /// <param name="obj">An object which has received the event.</param>
            /// <returns>true if the event is meant to be handled by <paramref name="obj"/>.</returns>
            public bool CanAccept(object src, object obj) {
                if (this.Attribute.HasFlag(EventAttribute.Directed)) {
                    if (!this.ValidTargets.Contains(obj)) {
                        return false;
                    }
                }
                if (this.Attribute.HasFlag(EventAttribute.Conditional)) {
                    if (!this.Condition(src, obj)) {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}