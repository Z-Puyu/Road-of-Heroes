using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Game.util;
using Game.util.events;
using Game.util.events.battle;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(HealEffect), "", nameof(Resource)), GlobalClass]
    public partial class HealEffect : CombatEffect {
        [Export] private bool IsPercentage { set; get; } = false;
        [Export] private ModifiableValueType HealTarget { set; get; } = ModifiableValueType.Health;
        [Export] private int MinHeal { set; get; }
        [Export] private int MaxHeal { set; get; }
        [Export] private int CriticalChance { set; get; } = 5;

        public override void Apply(Actor src, Actor target, bool crit = false) {
            crit = MathUtil.Randi(1, 100) <= this.CriticalChance;
            if (this.IsPercentage) {
                ModifiableValue stat = target.Get(this.HealTarget);
                int min = (int)Math.Ceiling(stat.MaxValue * this.MinHeal / 100.0);
                int max = (int)Math.Ceiling(stat.MaxValue * this.MaxHeal / 100.0);
                this.Publish(new HealingEvent(src, target, this.HealTarget, min, max, crit));
            } else {
                this.Publish(new HealingEvent(
                    src, target, this.HealTarget, this.MinHeal, this.MaxHeal, crit
                ));
            }
        }

        public override string GetDesc(Actor src, Actor target) {
            return this.ToString();
        }


        public override string ToString() {
            string amount;
            if (this.IsPercentage) {
                amount = this.MinHeal == this.MaxHeal 
                    ? $"{this.MinHeal}%" 
                    : $"{this.MinHeal}% to {this.MaxHeal}%";
                return $"Heal {amount} of maximum {this.HealTarget}";
            }
            amount = this.MinHeal == this.MaxHeal 
                ? $"{this.MinHeal}" 
                : $"{this.MinHeal} to {this.MaxHeal}";
            return $"Heal {amount} {this.HealTarget}";
        }
    }
}