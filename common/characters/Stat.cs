using System;
using System.Collections.Generic;
using Game.util;
using Godot;

namespace Game.common.characters {
    public partial class Stat : Resource {
        public enum Category {
            Health,
            Magicka,
            Sanity,
            Fatigue,
            Speed,
            Agility,
            Strength,
            Perception,
            Precision
        }

        [Export] public Category Type { get; set; }
        [Export] public int Value { set; get; }

        public Stat(Category type, int value) {
            this.Type = type;
            this.Value = value;
        }

        public static Dictionary<Category, Stat> Random() {
            Dictionary<Category, Stat> stats = [];
            for (Category cat = Category.Health; cat <= Category.Sanity; cat += 1) {
                stats.Add(cat, new Stat(cat, 100));
            }
            stats.Add(Category.Fatigue, new Stat(Category.Fatigue, 0));
            for (Category cat = Category.Speed; cat <= Category.Perception; cat += 1) {
                stats.Add(cat, new Stat(cat, Utilities.Randi(1, 5)));
            }
            stats.Add(Category.Precision, new Stat(Category.Precision, 0));
            return stats;
        }

        public static Stat operator+(Stat stat, int offset) {
            return new Stat(stat.Type, stat.Value + offset);
        }

        public static Stat operator-(Stat stat, int offset) {
            return new Stat(stat.Type, stat.Value - offset);
        }

        public static Stat operator*(Stat stat, double factor) {
            return new Stat(stat.Type, (int)Math.Round(stat.Value * factor));
        }

        public static Stat operator/(Stat stat, double divisor) {
            return new Stat(stat.Type, (int)Math.Round(stat.Value / divisor));
        }
    }
}