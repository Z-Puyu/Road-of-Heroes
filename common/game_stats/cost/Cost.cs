using System;
using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Cost), "", nameof(Resource)), GlobalClass]
    public abstract class Cost : Stat {
        [Export] public StatType StatType { set; get; }
        [Export] protected bool IsPercentage { set; get; } = false;

        public Cost(StatType costType, int value) : base(costType, value) {
            this.StatType = costType switch {
                StatType.HpCost => StatType.Health,
                StatType.MagickaCost => StatType.Magicka,
                StatType.SanityCost => StatType.Sanity,
                StatType.StaminaCost => StatType.Stamina,
                _ => StatType.Health
            };
        }

        public bool IsAffordable(Actor actor) {
            Stat stat = actor.Get(this.StatType);
            return stat.Value >= this.Compute(stat.MaxValue).Value;
        }

        public Cost Compute(int maxValue) {
            if (this.IsPercentage) {
                int value = (int)Math.Floor(maxValue * this.Value / 100.0);
                return this.Type switch {
                    StatType.HpCost => new HpCost(value),
                    StatType.MagickaCost => new MagickaCost(value),
                    StatType.SanityCost => new SanityCost(value),
                    StatType.StaminaCost => new StaminaCost(value),
                    _ => new HpCost(value)
                };
            }
            return this;
        }

        public override string ToString() {
            string stat = this.StatType switch {
                StatType.Health => "HP",
                StatType.Magicka => "Magicka",
                StatType.Sanity => "Sanity",
                StatType.Stamina => "Stamina",
                _ => ""
            };
            string value = this.IsPercentage ? $"{this.Value}% of maximum" : $"{this.Value}";
            return $"{value} {stat}";
        }
    }
}