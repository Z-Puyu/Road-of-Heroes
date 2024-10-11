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
            ModifiableValue s = actor.Get(this.TargetType());
            return s.Value >= this.ComputeFor(actor).Value;
        }

        public ModifiableValue ComputeFor(Actor actor) {
            ModifiableValueType stat = this.Type switch {
                CostType.Hp => ModifiableValueType.HpCost,
                CostType.Magicka => ModifiableValueType.MagickaCost,
                CostType.Sanity => ModifiableValueType.SanityCost,
                CostType.Stamina => ModifiableValueType.StaminaCost,
                _ => ModifiableValueType.HpCost
            };
<<<<<<< HEAD
            if (this.IsPercentage) {
                int value = (int)Math.Floor(actor.Get(stat).MaxValue * this.Value / 100.0);
                return new ModifiableValue(stat, value);
            }
            return new ModifiableValue(stat, this.Value);
=======
            return null;
            /* if (this.IsPercentage) {
                int value = (int)Math.Floor(actor.Get(stat).Value * this.Value / 100.0);
                return new Stat(stat, value);
            }
            return new Stat(stat, this.Value); */
>>>>>>> e50a7f5edd12946b0af396b056629f5c7b368333
        }

        public ModifiableValueType TargetType() {
            return this.Type switch {
                CostType.Hp => ModifiableValueType.Health,
                CostType.Magicka => ModifiableValueType.Magicka,
                CostType.Sanity => ModifiableValueType.Sanity,
                CostType.Stamina => ModifiableValueType.Stamina,
                _ => ModifiableValueType.Health
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