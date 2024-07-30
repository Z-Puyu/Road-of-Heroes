using System;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(MagicAttackEffect), "", nameof(Resource)), GlobalClass]
    public partial class MagicAttackEffect : Effect {
        [Export] private Vector2I DamageRange { set; get; } = Vector2I.Zero;

        public override void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not Character || target is not Character receiver) {
                return;
            }
            int dmg = target.Receive(
                Type.MagicAttack, src.Emit(Type.MagicAttack, (int)Math.Round(
                    crit ? this.DamageRange.Y * 1.5 
                         : Utilities.Randi(this.DamageRange.X, this.DamageRange.Y)
                ))
            );
            receiver.Update(Stat.Category.Health, -dmg);
        }

        public override string ToString() {
            return this.DamageRange.X == this.DamageRange.Y 
                    ? $"{this.DamageRange.X} magic damage" 
                    : $"{this.DamageRange.X} to {this.DamageRange.Y} magic damage";
        }
    }
}