using System.Collections.Generic;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(GainEffect), "", nameof(Resource)), GlobalClass]
    public partial class GainEffect : Effect {
        private readonly static Dictionary<Type, Stat.Category> stats = new
                Dictionary<Type, Stat.Category> {
            {Type.MagickaDrain, Stat.Category.Magicka},
            {Type.SanityDrain, Stat.Category.Sanity},
            {Type.FatigueLoss, Stat.Category.Fatigue}
        };

        [Export] private Vector2I GainRange { set; get; } = Vector2I.Zero;

        public override void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not Character || target is not Character receiver || 
                    (this.EffectType != Type.SanityRestore && 
                     this.EffectType != Type.MagickaRestore && 
                     this.EffectType != Type.FatigueGain)) {
                return;
            }
            if (GainEffect.stats.TryGetValue(this.EffectType, out Stat.Category stat)) {
                receiver.Update(stat, target.Receive(Type.PhysicalHeal, src.Emit(
                    Type.PhysicalHeal, Utilities.Randi(this.GainRange.X, this.GainRange.Y)
                )));
            }
        }
    }
}