using Game.common.stats;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(SkillCondition), "", nameof(Resource)), GlobalClass]
    public partial class SkillCondition : Resource {
        private enum Comparator {
            Equality, Greater, Less, GreaterOrEqual, LessOrEqual,
        }

        [Export] private StatType StatType { set; get; }
        [Export] private int Threshold { set; get; } = 0;
        [Export] private bool UsePercentage { set; get; } = false;
        [Export] private Comparator ComparisonMethod { set; get; }

        public bool Test(Actor actor) {
            Stat stat = actor.Get(this.StatType);
            if (this.UsePercentage) {
                Fraction threshold = new Fraction(this.Threshold, 100);
                Fraction ratio = new Fraction(stat.Value, stat.Value);
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
                    Comparator.Equality => stat.Value == this.Threshold,
                    Comparator.Greater => stat.Value > this.Threshold,
                    Comparator.Less => stat.Value < this.Threshold,
                    Comparator.GreaterOrEqual => stat.Value >= this.Threshold,
                    Comparator.LessOrEqual => stat.Value <= this.Threshold,
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