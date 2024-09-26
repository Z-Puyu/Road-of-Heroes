using System;
using System.Collections.Generic;

namespace Game.util.events {
    public abstract class ConditionalEvent<T> : EventArgs {
        private readonly Predicate<T> test;
        private readonly HashSet<T> targets;

        public ConditionalEvent(Predicate<T> test = null, HashSet<T> targets = null) {
            this.test = test ?? (t => true);
            this.targets = targets;
        }

        public bool HandledBy(object obj) {
            if (obj is not T t) {
                return false;
            }
            if (this.targets == null || this.targets.Count == 0) {
                return this.test(t);
            }
            return this.targets.Contains(t) && this.test(t);
        }
    }
}