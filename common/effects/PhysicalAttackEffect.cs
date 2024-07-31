using System;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(PhysicalAttackEffect), "", nameof(Resource)), GlobalClass]
    public partial class PhysicalAttackEffect : Effect {
        [Export] private int DamageMultiplier;

        public override void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not Character emitter || target is not Character receiver || 
                    (this.EffectType != Type.MeleeAttack && this.EffectType != Type.RangedAttack)) {
                return;
            }
            int strength = emitter.Get(Stat.Category.Strength);
            int dmg = target.Receive(
                this.EffectType, src.Emit(this.EffectType, Math.Max(1, (int)Math.Round(
                    (crit ? strength * 2 * 1.5 : Utilities.Randi(strength, strength * 2)) 
                    * this.DamageMultiplier / 100.0
                )))
            );
            receiver.Update(Stat.Category.Health, -dmg);
        }

        public override string ToDesc(Character character) {
            string attack = this.EffectType switch {
                Type.MeleeAttack => "melee damage",
                Type.RangedAttack => "ranged damage",
                _ => ""
            };
            int strength = character.Get(Stat.Category.Strength);
            int min = Math.Max(1, (int)Math.Round(strength * this.DamageMultiplier / 100.0));
            int max = Math.Max(1, (int)Math.Round(strength * 2 * this.DamageMultiplier / 100.0));
            return $"{(min == max ? $"{min}" : $"{min} to {max}")} {attack}";
        }
    }
}