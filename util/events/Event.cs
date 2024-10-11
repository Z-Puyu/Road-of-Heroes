using Game.util.math;
using System;
using System.Collections.Generic;

namespace Game.util.events {
    /// <summary>
    /// Wrapper class for a C# event used to transmit data between game objects.
    /// </summary>
    /// <typeparam name="E">The type of the event.</typeparam>
    public sealed class Event<E> where E : EventArgs {
        [Flags]
        private enum EventAttribute {
            None = 0,
            Directed = 1,
            Conditional = 2,
        }

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
            E e, HashSet<object> targets, Predicate<object, object> condition
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