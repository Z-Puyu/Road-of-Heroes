global using EventManager = Game.util.events.EventChannel<System.EventArgs>.EventManager;
using System;
using System.Collections.Generic;
using Game.util.events.system;
using Godot;


namespace Game.util.events {
    public static class EventChannel<E> where E : EventArgs {
        public delegate void EventListener(E e);
        private static EventListener OnEvent { set; get; } = _ => {};
        private static EventChannel<EventArgs>.EventListener OnGeneralEvent { set; get; } = _ => {};

        /// <summary>
        /// Register an event using 
        /// </summary>
        /// <param name="handler"></param>
        private static void Register(EventChannel<EventArgs>.EventListener handler) {
            EventChannel<E>.OnGeneralEvent += handler;
        } 

        private static void Unregister(EventChannel<EventArgs>.EventListener handler) {
            EventChannel<E>.OnGeneralEvent -= handler;
        }

        private static void Register(EventListener handler) {
            EventChannel<E>.OnEvent += handler;
        } 

        private static void Unregister(EventListener handler) {
            EventChannel<E>.OnEvent -= handler;
        }

        public static void Publish(E e) {
            if (e != null) {
                EventChannel<E>.OnEvent.Invoke(e);
                EventChannel<E>.OnGeneralEvent.Invoke(e);
            }
        }

        public class EventManager {
            private readonly object root;
            private readonly Dictionary<Type, Action> unregisterers = [];
            private readonly Dictionary<Type, Action> registerers = [];
            private readonly HashSet<Type> muted = [];

            public EventManager(object root) {
                this.root = root;
                EventChannel<RecycleObjectEvent>.Register(this.OnRecycleNode);
            }

            // <summary>
            /// Subscribes a handling function <paramref name="handler"/> to event type <typeparamref name="F"/>. Every event type can be associated to at most one handler function. <param name="handler"/> used in this method can accept all types of events.
            /// </summary>
            /// <typeparam name="F">The type of the event.</typeparam>
            /// <param name="handler">The handler function</param>
            public void Subscribe<F>(EventChannel<EventArgs>.EventListener handler) where F : EventArgs {
                Type t = typeof(F);
                if (this.Mute<F>()) {
                    this.unregisterers.Add(t, () => EventChannel<E>.Unregister(handler));
                }
                EventChannel<E>.Register(handler);
                if (this.registerers.ContainsKey(t)) {
                    this.registerers[t] = () => EventChannel<E>.Register(handler);
                } else {
                    this.registerers.Add(t, () => EventChannel<E>.Register(handler));
                }
            }

            /// <summary>
            /// Subscribes a handling function <paramref name="handler"/> to event type <typeparamref name="F"/>. Every event type can be associated to at most one handler function.
            /// </summary>
            /// <typeparam name="F">The type of the event.</typeparam>
            /// <param name="handler">The handler function</param>
            public void Subscribe<F>(EventChannel<F>.EventListener handler) where F : EventArgs {
                Type t = typeof(F);
                if (this.Mute<F>()) {
                    this.unregisterers.Add(t, () => EventChannel<F>.Unregister(handler));
                }
                EventChannel<F>.Register(handler);
                if (this.registerers.ContainsKey(t)) {
                    this.registerers[t] = () => EventChannel<F>.Register(handler);
                } else {
                    this.registerers.Add(t, () => EventChannel<F>.Register(handler));
                }
            }

            /// <summary>
            /// Stop listening to events of type <typeparamref name="F"/>.
            /// </summary>
            /// <typeparam name="F">The event type.</typeparam>
            /// <returns>true if the event is currently being listened to.</returns>
            public bool Mute<F>() where F : EventArgs {
                Type t = typeof(F);
                if (this.unregisterers.Remove(t, out Action unregister)) {
                    unregister();
                    this.muted.Add(t);
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Revive subscription to events of type <typeparamref name="F"/>.
            /// </summary>
            /// <typeparam name="F">The event type.</typeparam>
            public void Revive<F>() {
                Type t = typeof(F);
                if (this.muted.Remove(t)) {
                    this.registerers[t]();
                }
            }

            /// <summary>
            /// Stop listening to all events.
            /// </summary>
            public void MuteAll() {
                foreach (KeyValuePair<Type, Action> pair in this.unregisterers) {
                    this.muted.Add(pair.Key);
                    pair.Value();
                }
            }

            /// <summary>
            /// Resume subscription to all previously muted events.
            /// </summary>
            public void ReviveAll() {
                foreach (Type t in this.muted) {
                    this.registerers[t]();
                }
            }

            private void OnRecycleNode(RecycleObjectEvent e) {
                if (e.HandledBy(this.root)) {
                    foreach (Action unregister in this.unregisterers.Values) {
                        unregister();
                    }
                    this.unregisterers.Clear();
                    this.registerers.Clear();
                    this.muted.Clear();
                    EventChannel<RecycleObjectEvent>.Unregister(this.OnRecycleNode);
                    if (this.root is Node node) {
                        node.QueueFree();
                    }
                }
            }
        }
    }
}