using System;
using Game.common.stats;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(Modifier), "", nameof(Resource)), GlobalClass]
    public abstract partial class Modifier : Resource {
        private enum ValueType { Current, Max, Min }
        [Export] public int TimeToLast = 0;
        [Export] public bool UsePercentage { get; set; } = false;
        [Export] private int Value { set; get; } = 1;
        [Export] public StatType TargetStat { get; set; }
        [Export] private ValueType TargetValue { get; set; } = ValueType.Current;

        public Modifier() {}

        public (int, int, int) ToTriplet() {
            return this.TargetValue switch {
                ValueType.Current => (this.Value, 0, 0),
                ValueType.Max => (0, this.Value, 0),
                ValueType.Min => (0, 0, this.Value),
                _ => (0, 0, 0)
            };
        }
    }
}