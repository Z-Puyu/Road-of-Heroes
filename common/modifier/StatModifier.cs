using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(StatModifier), "", nameof(Resource)), GlobalClass]
    public partial class StatModifier : Modifier {
        [Export] public Stat.Category TargetStat { get; set; }

        public StatModifier() : base() {}

        public override string ToString() {
            string stat = this.TargetStat switch {
                Stat.Category.Health => "HP",
                Stat.Category.Magicka => "Magicka",
                Stat.Category.Sanity => "San",
                Stat.Category.Fatigue => "Fatigue",
                Stat.Category.Agility => "Agility",
                Stat.Category.Speed => "Speed",
                Stat.Category.Strength => "Strength",
                Stat.Category.Perception => "Perception",
                Stat.Category.Precision => "Precision",
                Stat.Category.BleedResist => "Bleed resistance",
                Stat.Category.PoisonResist => "Poison resistance",
                Stat.Category.BurnResist => "Burn resistance",
                Stat.Category.BlightResist => "Blight resistance",
                Stat.Category.StunResist => "Stun resistance",
                _ => "",
            };
            if (this.TargetStat >= Stat.Category.BleedResist && 
                this.TargetStat <= Stat.Category.StunResist) {
                return $"{stat} {(this.Value >= 0 ? "+" : "")}{this.Value}%";
            }
            return $"{stat} {(this.Value >= 0 ? "+" : "")}" 
                 + $"{this.Value}{(this.UsePercentage ? "%" : "")}";
        }
    }
}