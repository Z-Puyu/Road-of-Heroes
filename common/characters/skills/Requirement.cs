using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(Requirement), "", nameof(Resource)), GlobalClass]
    public partial class Requirement : Resource {
        private enum Comparator {
            Equality, Greater, Less
        }

        [Export] private Stat.Category StatType { set; get; }
        [Export] private int Value { set; get; } = 0;
        [Export] private Comparator ComparisonMethod { set; get; }

        public bool Test(Character character) {
            if (character.Get(this.StatType, out Stat value)) {
                return this.ComparisonMethod switch {
                    Comparator.Equality => (double)value.Value / (double)value.MaxValue == this.Value,
                    Comparator.Greater => (double)value.Value / (double)value.MaxValue > this.Value,
                    Comparator.Less => (double)value.Value / (double)value.MaxValue < this.Value,
                    _ => false
                };
            }
            return true;
        }

        public override string ToString() {
            string stat = this.StatType switch {
                Stat.Category.Health => "HP",
                Stat.Category.Magicka => "MG",
                Stat.Category.Sanity => "SAN",
                Stat.Category.Fatigue => "FG",
                _ => ""
            };
            string comparator = this.ComparisonMethod switch {
                Comparator.Equality => "=",
                Comparator.Greater => ">",
                Comparator.Less => "<",
                _ => ""
            };
            return $"{stat} {comparator} {this.Value}";
        }
    }
}