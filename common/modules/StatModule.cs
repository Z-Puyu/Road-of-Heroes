using System.Collections.Generic;
using Game.common.stats;
using Godot;

namespace Game.common.modules {
    /// <summary>
    /// Encapsulates a manager of game object stats.
    /// </summary>
    public partial class StatModule : Node {
        private Dictionary<StatType, Stat> Stats { get; init; } = [];

        /// <summary>
        /// Attempts to retrieve the stat entry of type <paramref name="t"/>. 
        /// The variable <paramref name="s"/> will contain the stat retrieved if it exists.
        /// </summary>
        /// <param name="t">The type of the stat entry.</param>
        /// <param name="s">The stat entry retrieved.</param>
        /// <returns>true if the stat entry of type <paramref name="t"/> exists.</returns>
        public bool TryGet(StatType t, out Stat s) {
            return this.Stats.TryGetValue(t, out s);
        }

        /// <summary>
        /// Attempts to update the stat entry of type <paramref name="t"/> by the <paramref name="offset"/> value. 
        /// The variable <paramref name="s"/> will contain the updated stat if it exists.
        /// </summary>
        /// <param name="t">The type of the stat entry.</param>
        /// <param name="s">The updated stat entry.</param>
        /// <param name="offset">The offset in current value.</param>
        /// <returns>true if the stat entry of type <paramref name="t"/> exists.</returns>
        public bool TryUpdate(StatType t, out Stat s, int offset) {
            if (this.Stats.TryGetValue(t, out s)) {
                //s += (offset, maxOffset, minOffset);
                this.Stats[t] = s;
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
            return this.Stats.TryAdd(s.Type, s);
        }

        public bool Contains(StatType t) {
            return this.Stats.ContainsKey(t);
        }
    }
}