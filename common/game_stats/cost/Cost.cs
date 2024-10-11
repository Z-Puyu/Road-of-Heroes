using System;
using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Cost), "", nameof(Resource)), GlobalClass]
    public partial class Cost : Resource {
        private enum CostType { Hp, Magicka, Sanity, Stamina }
        [Export] private CostType Type { set; get; }
        [Export] private int Value { set; get; } = 0;
        [Export] private bool IsPercentage { set; get; } = false;

        public Cost() {}

        public bool IsAffordable(Actor actor) {
            Stat s = actor.Get(this.TargetType());
            return s.Value >= this.ComputeFor(actor).Value;
        }

        public Stat ComputeFor(Actor actor) {
            StatType stat = this.Type switch {
                CostType.Hp => StatType.HpCost,
                CostType.Magicka => StatType.MagickaCost,
                CostType.Sanity => StatType.SanityCost,
                CostType.Stamina => StatType.StaminaCost,
                _ => StatType.HpCost
            };
            return null;
            /* if (this.IsPercentage) {
                int value = (int)Math.Floor(actor.Get(stat).Value * this.Value / 100.0);
                return new Stat(stat, value);
            }
            return new Stat(stat, this.Value); */
        }

        public StatType TargetType() {
            return this.Type switch {
                CostType.Hp => StatType.Health,
                CostType.Magicka => StatType.Magicka,
                CostType.Sanity => StatType.Sanity,
                CostType.Stamina => StatType.Stamina,
                _ => StatType.Health
            };
        }

        public override string ToString() {
            string stat = this.Type switch {
                CostType.Hp => "HP",
                CostType.Magicka => "Magicka",
                CostType.Sanity => "Sanity",
                CostType.Stamina => "Stamina",
                _ => ""
            };
            string value = this.IsPercentage ? $"{this.Value}% of maximum" : $"{this.Value}";
            return $"{value} {stat}";
        }
    }
}