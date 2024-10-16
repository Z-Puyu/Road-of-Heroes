using System.Collections.Generic;
using Game.common.stats;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(SkillCondition), "", nameof(Resource)), GlobalClass]
    public partial class SkillCondition : Resource {
        private readonly static Dictionary<StatType, StatType> CAPPED_STATS = new Dictionary<StatType, StatType>{
            {StatType.Health, StatType.MaxHP},
            {StatType.Magicka, StatType.MaxMagicka},
            {StatType.Sanity, StatType.MaxSanity},
            {StatType.Stamina, StatType.Stamina}
        };

        private enum Comparator {
            Equality, Greater, Less, GreaterOrEqual, LessOrEqual
        }

        [Export] private StatType StatType { set; get; }
        [Export] private int Threshold { set; get; } = 0;
        [Export] private bool UsePercentage { set; get; } = false;
        [Export] private Comparator ComparisonMethod { set; get; }

        public bool Test(Actor actor) {
            bool isCapped = CAPPED_STATS.ContainsKey(this.StatType);
            int stat = actor.Get(this.StatType);
            if (this.UsePercentage && isCapped) {
                Fraction threshold = new Fraction(this.Threshold, 100);
                Fraction ratio = new Fraction(stat, actor.Get(CAPPED_STATS[this.StatType]));
                return this.ComparisonMethod switch {
                    Comparator.Equality => ratio == threshold,
                    Comparator.Greater => ratio > threshold,
                    Comparator.Less => ratio < threshold,
                    Comparator.GreaterOrEqual => ratio >= threshold,
                    Comparator.LessOrEqual => ratio <= threshold,
                    _ => false
                };
            } else {
                return this.ComparisonMethod switch {
                    Comparator.Equality => stat == this.Threshold,
                    Comparator.Greater => stat > this.Threshold,
                    Comparator.Less => stat < this.Threshold,
                    Comparator.GreaterOrEqual => stat >= this.Threshold,
                    Comparator.LessOrEqual => stat <= this.Threshold,
                    _ => false
                };
            }
        }

        public override string ToString() {
            string stat = this.StatType switch {
                StatType.Health => "HP",
                StatType.Magicka => "Magicka",
                StatType.Sanity => "Sanity",
                StatType.Stamina => "Stamina",
                _ => ""
            };
            string comparator = this.ComparisonMethod switch {
                Comparator.Equality => "=",
                Comparator.Greater => ">",
                Comparator.Less => "<",
                Comparator.GreaterOrEqual => "≥",
                Comparator.LessOrEqual => "≤",
                _ => ""
            };
            return $"{stat} {comparator} {this.Threshold}{(this.UsePercentage ? "% of the maximum value" : "")}";
        }
    }
}