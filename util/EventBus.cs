using System;
using System.Collections.Generic;

namespace Game.util {
	public static class EventBus {
		private static readonly Dictionary<Type, EventHandler> subscribers = [];
		private static readonly Dictionary<object, Dictionary<Type, HashSet<EventHandler>>> lookup = [];

		public static void Subscribe<T>(this object src, EventHandler eventHandler) where T : EventArgs {
			Type eventType = typeof(T);
			if (!EventBus.subscribers.TryAdd(eventType, eventHandler)) {
				EventBus.subscribers[eventType] += eventHandler;
			}
			if (EventBus.lookup.TryGetValue(src, out Dictionary<Type, HashSet<EventHandler>> dict)) {
				if (!dict.TryAdd(eventType, [eventHandler])) {
					dict[eventType].Add(eventHandler);
				}
			} else {
				EventBus.lookup.Add(src, new Dictionary<Type, HashSet<EventHandler>> {
					{eventType, [eventHandler]}
				});
			}
		}

		public static void Unsubscribe<T>(this object src, EventHandler eventHandler) where T : EventArgs {
			Type eventType = typeof(T);
			if (EventBus.subscribers.ContainsKey(eventType)) {
				EventBus.subscribers[eventType] -= eventHandler;
			}
			if (EventBus.lookup.TryGetValue(src, out Dictionary<Type, HashSet<EventHandler>> dict)) {
                dict[eventType].Remove(eventHandler);
			}
		}

		public static void Publish(this object sender, EventArgs e) {
			Type eventType = e.GetType();
			if (EventBus.subscribers.TryGetValue(eventType, out EventHandler listeners)) {
				listeners(sender, e);
			}
		}

		public static void UnsubscribeAllEvents(this object src) {
			if (EventBus.lookup.TryGetValue(src, out Dictionary<Type, HashSet<EventHandler>> dict)) {
				foreach (Type eventType in dict.Keys) {
					foreach (EventHandler f in dict[eventType]) {
						EventBus.subscribers[eventType] -= f;
					}
				}
				EventBus.lookup.Remove(src);
			}
		}
	}
}
