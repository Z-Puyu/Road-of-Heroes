using System;
using System.Collections.Generic;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Stat), "", nameof(Resource)), GlobalClass]
    public partial class Stat : Resource {
        public enum Category {
            Health,
            Magicka,
            Sanity,
            Fatigue,
            Agility,
            Speed,
            Strength,
            Perception,
            Precision,
            BleedResist,
            PoisonResist,
            BurnResist,
            BlightResist,
            StunResist
        }

        [Export] public Category Type { get; set; }
        [Export] public int Value { set; get; }
        [Export] public int MaxValue { set; get; } = int.MaxValue;
        [Export] public int MinValue { set; get; } = 0;

        public Stat() {}

        public Stat(Category type, int value, int maxValue = int.MaxValue, int minValue = 0) {
            this.Type = type;
            this.Value = value;
            this.MaxValue = maxValue;
            this.MinValue = minValue;
        }

        public static Dictionary<Category, Stat> Random() {
            Dictionary<Category, Stat> stats = [];
            for (Category cat = Category.Health; cat <= Category.Magicka; cat += 1) {
                int value = Utilities.Randi(15, 30);
                stats.Add(cat, new Stat(cat, value, value));
            }
            stats.Add(Category.Sanity, new Stat(Category.Sanity, 100, 100));
            stats.Add(Category.Fatigue, new Stat(Category.Fatigue, 0, 100));
            stats.Add(Category.Agility, new Stat(Category.Agility, Utilities.Randi(5, 10), 95, 5));
            for (Category cat = Category.Speed; cat <= Category.Strength; cat += 1) {
                stats.Add(cat, new Stat(cat, Utilities.Randi(1, 5)));
            }
            stats.Add(Category.Perception, new Stat(Category.Perception, Utilities.Randi(1, 5), 33));
            stats.Add(Category.Precision, new Stat(Category.Precision, 0));
            for (Category cat = Category.BleedResist; cat <= Category.StunResist; cat += 1) {
                stats.Add(cat, new Stat(cat, Utilities.Randi(10, 50), 100, 10));
            }
            return stats;
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
    }
}