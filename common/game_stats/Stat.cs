using System;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Stat), "", nameof(Resource)), GlobalClass]
    public partial class Stat : Resource {
        [Export] public StatType Type { get; set; }
        [Export] public int Value { set; get; } = 0;
        [Export] public int MaxValue { set; get; } = int.MaxValue;
        [Export] public int MinValue { set; get; } = 0;

        public Stat() {}

        public Stat(StatType type, int value = 0, int maxValue = int.MaxValue, int minValue = 0) {
            this.Type = type;
            this.Value = value;
            this.MaxValue = maxValue;
            this.MinValue = minValue;
        }

        public static Stat Random(StatType type, (int, int) range, int min = 0, int max = int.MaxValue) {
            return new Stat(type, Utilities.Randi(range), min, max);
        } 

        public static Stat operator+(Stat stat, (int, int, int) offset) {
            int maxValue = Math.Max(stat.MaxValue + offset.Item2, 0);
            int minValue = Math.Max(stat.MinValue + offset.Item3, 0);
            int value = Math.Clamp(stat.Value + offset.Item1, minValue, maxValue);
            return new Stat(stat.Type, value, maxValue, minValue);
        }

        public static Stat operator-(Stat stat, (int, int, int) offset) {
            int maxValue = Math.Max(stat.MaxValue - offset.Item2, 0);
            int minValue = Math.Max(stat.MinValue + offset.Item3, 0);
            int value = Math.Clamp(stat.Value + offset.Item1, minValue, maxValue);
            return new Stat(stat.Type, value, maxValue);
        }

        public static Stat operator*(Stat stat, (double, double, double) factor) {
            int maxValue = (int)Math.Round(Math.Max(stat.MaxValue * factor.Item2, 0));
            int minValue = (int)Math.Round(Math.Max(stat.MinValue * factor.Item3, 0));
            int value = Math.Clamp((int)Math.Round(stat.Value * factor.Item1), minValue, maxValue);
            return new Stat(stat.Type, value, maxValue, minValue);
        }

        public static Stat operator/(Stat stat, (double, double, double) divisor) {
            int maxValue = (int)Math.Round(Math.Max(stat.MaxValue / divisor.Item2, 0));
            int minValue = (int)Math.Round(Math.Max(stat.MinValue / divisor.Item3, 0));
            int value = Math.Clamp((int)Math.Round(stat.Value / divisor.Item1), minValue, maxValue);
            return new Stat(stat.Type, value, maxValue, minValue);
        }

        public override string ToString() {
            return $"{this.Value} {this.Type}, min: {this.MinValue}, max: {this.MaxValue}";
        }
    }
}