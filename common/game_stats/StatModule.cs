using System.Collections.Generic;
using Game.common.stats;
using Godot;

namespace Game.common.stats {
    /// <summary>
    /// Encapsulates a manager of game object stats.
    /// </summary>
    public partial class StatModule : Node {
        protected readonly Dictionary<StatType, Stat> stats = [];

        public bool TryGet(StatType t, out Stat s) {
            return this.stats.TryGetValue(t, out s);
        }

        public bool TryUpdate(StatType t, out Stat s, int offset, int maxOffset = 0, int minOffset = 0) {
            if (this.stats.TryGetValue(t, out s)) {
                s += (offset, maxOffset, minOffset);
                return true;
            }
            return false;
        }

        public bool TryAdd(StatType t, in Stat s) {
            return this.stats.TryAdd(t, s);
        }

        public bool Contains(StatType t) {
            return this.stats.ContainsKey(t);
        }
    }
}