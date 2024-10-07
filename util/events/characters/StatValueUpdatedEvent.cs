using System;
using Game.common.characters;
using Game.common.stats;

namespace Game.util.events.characters {
    public class ModifiableValueValueUpdatedEvent : EventArgs {
        private readonly ModifiableValueType stat;
        private readonly int value;
        private readonly int maxValue;

        public ModifiableValueValueUpdatedEvent(ModifiableValueType stat, int value, int maxValue) {
            this.stat = stat;
            this.value = value;
            this.maxValue = maxValue;
        }

        public ModifiableValueType ModifiableValue => stat;
        public int Value => value;
        public int MaxValue => maxValue;
    }
}