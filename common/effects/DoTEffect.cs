using Game.common.characters;
using Game.common.effects.eot;
using Game.common.stats;
using Game.util;
using Game.util.events;
using Game.util.events.battle;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(DoTEffect), "", nameof(Resource)), GlobalClass]
    public partial class DoTEffect : CombatEffect {
        [Export] private DoT DoT { set; get; }
        [Export] private int SuccessChance { set; get; } = 100;

        public override void Apply(Actor src, Actor target, bool crit = false) {
            if (this.DoT != null) {
                int resistance = this.DoT.EffectType switch {
                    OverTimeEffect.Bleed => target.Get(StatType.BleedResist).Value,
                    OverTimeEffect.Blight => target.Get(StatType.BlightResist).Value,
                    OverTimeEffect.Burn => target.Get(StatType.BurnResist).Value,
                    OverTimeEffect.Poison => target.Get(StatType.PoisonResist).Value,
                    OverTimeEffect.Stun => target.Get(StatType.StunResist).Value,
                    _ => 0
                };
                int chance = src.Filter(new DoTSuccessChance(
                    this.DoT.EffectType, this.SuccessChance - resistance + (crit ? 50 : 0)
                )).Value;
                // this.Publish(new ReceiveEffectEvent(this.DoT, chance, target));
            }
        }

        public override string GetDesc(Actor src, Actor target) {
            return this.ToString();
        }


        public override string ToString() {
            return $"{this.SuccessChance}% chance of {this.DoT}";
        }
    }
}