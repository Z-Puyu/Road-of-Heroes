using System;
using Game.common.characters;
using Game.common.stats;

namespace Game.util.events.characters {
    public class StatValueUpdatedEvent : EventArgs {
        private readonly StatType stat;
        private readonly int value;
        private readonly int maxValue;

        public StatValueUpdatedEvent(StatType stat, int value, int maxValue) {
            this.stat = stat;
            this.value = value;
            this.maxValue = maxValue;
        }

        public StatType Stat => stat;
        public int Value => value;
        public int MaxValue => maxValue;
    }
}