using System;
using System.Collections.Generic;
using Game.common.characters;
using Game.util.events;
using Game.util.events.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Cost), "", nameof(Resource)), GlobalClass]
    public partial class Cost : Stat {
        private enum Category { 
            Hp = StatType.HpCost, 
            Magicka = StatType.MagickaCost, 
            Sanity = StatType.SanityCost, 
            Stamina = StatType.StaminaCost 
        }

        private static Dictionary<StatType, StatType> BaseStats { get; } = new Dictionary<StatType, StatType> {
            {StatType.HpCost, StatType.Health},
            {StatType.MagickaCost, StatType.Magicka},
            {StatType.SanityCost, StatType.Sanity},
            {StatType.StaminaCost, StatType.Stamina}
        };

        private Category costType;

        [Export] private Category CostType { 
            get => costType; 
            set {
                costType = value;
                Type = (StatType)value;
            } 
        }
        [Export] private bool IsPercentage { set; get; } = false;

        public Cost() {}

        public Cost(StatType type, int value, bool isPercentage) : base(type, value) {
            this.IsPercentage = isPercentage;
            this.CostType = (Category)type;
        }

        public bool IsAffordable(Actor actor) {
            return actor.Get(Cost.BaseStats[this.Type]) >= this.ValueOf(actor).Value;
        }

        private Stat ValueOf(Actor actor) {
            int value = this.IsPercentage ? (int)Math.Floor(
                actor.Get(Cost.BaseStats[this.Type]) * this.Value / 100.0
            ) : this.Value;
            return new Stat(this.Type, -value);
        }

        public void ConsumeBy(Actor actor) {
            Stat change = this.ValueOf(actor);
            this.Publish(new UpdateStatsEvent(new Dictionary<StatType, int> {
                    {this.Type, change.Value}
            }));
        }

        public override string ToString() {
            string value = this.IsPercentage ? $"{this.Value}% of maximum" : $"{this.Value}";
            return $"{value} {this.Type}";
        }
    }
}