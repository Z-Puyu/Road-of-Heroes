using System;
using Game.util.math;
using Godot;

namespace Game.common.stats {
    public partial class ModifiableValue : Resource {
        [Export] public ModifiableValueType Type { get; set; }
        [Export] public int Value { set; get; } = 0;
        [Export] public int MaxValue { set; get; } = int.MaxValue;

        public ModifiableValue(ModifiableValueType type, int value = 0, int maxValue = int.MaxValue) {
            this.Type = type;
            this.Value = value;
            this.MaxValue = maxValue;
        }

        public static ModifiableValue operator+(ModifiableValue stat, (int, int) offset) {
            int maxValue = Math.Max(stat.MaxValue + offset.Item2, 0);
            int value = Math.Clamp(stat.Value + offset.Item1, 0, maxValue);
            return new ModifiableValue(stat.Type, value, maxValue);
        }

        public static ModifiableValue operator-(ModifiableValue stat, (int, int) offset) {
            int maxValue = Math.Max(stat.MaxValue - offset.Item2, 0);
            int value = Math.Clamp(stat.Value + offset.Item1, 0, maxValue);
            return new ModifiableValue(stat.Type, value, maxValue);
        }

        public static ModifiableValue operator*(ModifiableValue stat, (double, double) factor) {
            int maxValue = (int)Math.Round(Math.Max(stat.MaxValue * factor.Item2, 0));
            int value = Math.Clamp((int)Math.Round(stat.Value * factor.Item1), 0, maxValue);
            return new ModifiableValue(stat.Type, value, maxValue);
        }

        public static ModifiableValue operator/(ModifiableValue stat, (double, double, double) divisor) {
            int maxValue = (int)Math.Round(Math.Max(stat.MaxValue / divisor.Item2, 0));
            int value = Math.Clamp((int)Math.Round(stat.Value / divisor.Item1), 0, maxValue);
            return new ModifiableValue(stat.Type, value, maxValue);
        }

        public override string ToString() {
            return $"{this.Type}: {this.Value} / {this.MaxValue}";
        }
    }
}