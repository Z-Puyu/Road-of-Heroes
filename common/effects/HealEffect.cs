using System;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(HealEffect), "", nameof(Resource)), GlobalClass]
    public partial class HealEffect : Effect {
        [Export] private Vector2I HealRange { set; get; } = Vector2I.Zero;
        [Export] private int CriticalChance { set; get; } = 5;

        public override void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not Character || target is not Character receiver || 
                    (this.EffectType != Type.PhysicalHeal && this.EffectType != Type.MagicHeal)) {
                return;
            }
            int dice = Utilities.Randi(1, 100);
            int amount = target.Receive(
                Type.PhysicalHeal, src.Emit(Type.PhysicalHeal, (int)Math.Round(
                    dice <= this.CriticalChance 
                    ? this.HealRange.Y * 1.5 
                    : Utilities.Randi(this.HealRange.X, this.HealRange.Y)
                ))
            );
            receiver.Update(Stat.Category.Health, amount);
        }
    }
}