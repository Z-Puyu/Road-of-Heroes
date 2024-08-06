using System;
using Game.common.characters;

namespace Game.util.events.characters {
    public class StatValueUpdatedEvent : EventArgs {
        private readonly Stat.Category stat;
        private readonly int value;
        private readonly int maxValue;

        public StatValueUpdatedEvent(Stat.Category stat, int value, int maxValue) {
            this.stat = stat;
            this.value = value;
            this.maxValue = maxValue;
        }

        public Stat.Category Stat => stat;
        public int Value => value;
        public int MaxValue => maxValue;
    }
}