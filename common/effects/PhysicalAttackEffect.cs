using System;
using Game.common.characters;
using Game.util;
using Godot;

namespace Game.common.effects {
    public partial class PhysicalAttackEffect : Effect {
        [Export] private int DamageMultiplier;

        public override void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not Character emitter || target is not Character receiver || 
                    (this.EffectType != Type.MeleeAttack && this.EffectType != Type.RangedAttack)) {
                return;
            }
            int strength = emitter.Get(Stat.Category.Strength);
            int dmg = target.Receive(
                this.EffectType, src.Emit(this.EffectType, (int)Math.Round(
                    (crit ? strength * 2 * 1.5 : Utilities.Randi(strength, strength * 2)) 
                    * this.DamageMultiplier / 100.0
                ))
            );
            receiver.Update(Stat.Category.Health, -dmg);
        }
    }
}