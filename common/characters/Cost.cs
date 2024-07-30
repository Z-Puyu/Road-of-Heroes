using System;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Cost), "", nameof(Resource)), GlobalClass]
    public partial class Cost : Resource {
        [Export] public Stat.Category StatType { set; get; }
        [Export] private int Amount { set; get; }
        [Export] private bool IsPercentage { set; get; } = false;

        public Cost() {}

        public int Compute(int maxValue) {
            return this.IsPercentage ? (int)Math.Round(maxValue * this.Amount / 100.0) 
                                     : this.Amount;
        }

        public override string ToString() {
            string stat = this.StatType switch {
                Stat.Category.Health => "HP",
                Stat.Category.Magicka => "MG",
                Stat.Category.Sanity => "SAN",
                Stat.Category.Fatigue => "FG",
                _ => ""
            };
            string value = this.IsPercentage ? $"{this.Amount}% of maximum" : $"{this.Amount}";
            return $"{value} {stat}";
        }
    }
}