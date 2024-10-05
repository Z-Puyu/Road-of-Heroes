using System.Threading.Tasks;
using Game.common.characters;
using Game.common.effects.eot;
using Game.common.stats;
using Game.common.tokens;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(DoTEffect), "", nameof(Resource)), GlobalClass]
    public partial class DoTEffect : Effect<Actor, Actor> {
        [Export] private DoT DoT { set; get; }
        [Export] private int SuccessChance { set; get; } = 100;

        public override async Task Apply(Actor src, Actor target, bool crit = false) {
            if (this.DoT != null) {
                int resistance = this.DoT.EffectType switch {
                    OverTimeEffect.Bleed => target.Get(StatType.BleedResist).Value,
                    OverTimeEffect.Blight => target.Get(StatType.BlightResist).Value,
                    OverTimeEffect.Burn => target.Get(StatType.BurnResist).Value,
                    OverTimeEffect.Poison => target.Get(StatType.PoisonResist).Value,
                    OverTimeEffect.Stun => target.Get(StatType.StunResist).Value,
                    _ => 0
                };
                src.ApplyEffect(
                    this.DoT, target, 
                    this.SuccessChance - resistance + (crit ? 50 : 0)
                );
            }
        }

        public override string ToDesc(Actor actor) {
            return this.ToString();
        }


        public override string ToString() {
            return $"{this.SuccessChance}% chance of {this.DoT}";
        }
    }
}