using System.Collections.Generic;
using Game.common.stats;
using Godot;

namespace Game.common.stats {
    /// <summary>
    /// Encapsulates a manager of game object stats.
    /// </summary>
    public partial class StatModule : Node {
        protected readonly Dictionary<StatType, Stat> stats = [];

        /// <summary>
        /// Attempts to retrieve the stat entry of type <paramref name="t"/>. 
        /// The variable <paramref name="s"/> will contain the stat retrieved if it exists.
        /// </summary>
        /// <param name="t">The type of the stat entry.</param>
        /// <param name="s">The stat entry retrieved.</param>
        /// <returns>true if the stat entry of type <paramref name="t"/> exists.</returns>
        public bool TryGet(StatType t, out Stat s) {
            return this.stats.TryGetValue(t, out s);
        }

        /// <summary>
        /// Attempts to update the stat entry of type <paramref name="t"/> by the offset tuple 
        /// (<paramref name="offset"/>, <paramref name="maxOffset"/>, <paramref name="minOffset"/>). 
        /// The variable <paramref name="s"/> will contain the updated stat if it exists.
        /// </summary>
        /// <param name="t">The type of the stat entry.</param>
        /// <param name="s">The updated stat entry.</param>
        /// <param name="offset">The offset in current value.</param>
        /// <param name="maxOffset">The offset in maximum value.</param>
        /// <param name="minOffset">The offset in minimum value.</param>
        /// <returns>true if the stat entry of type <paramref name="t"/> exists.</returns>
        public bool TryUpdate(StatType t, out Stat s, int offset, int maxOffset = 0, int minOffset = 0) {
            if (this.stats.TryGetValue(t, out s)) {
                s += (offset, maxOffset, minOffset);
                this.stats[t] = s;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to add a new stat entry.
        /// </summary>
        /// <param name="s">The stat entry.</param>
        /// <returns>true if there is no stat entry with the same type.</returns>
        public bool TryAdd(in Stat s) {
            return this.stats.TryAdd(s.Type, s);
        }

        public bool Contains(StatType t) {
            return this.stats.ContainsKey(t);
        }
    }
}