using System.Collections.Generic;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(DrainEffect), "", nameof(Resource)), GlobalClass]
    public partial class DrainEffect : Effect {
        private readonly static Dictionary<Type, Stat.Category> stats = new
                Dictionary<Type, Stat.Category> {
            {Type.MagickaDrain, Stat.Category.Magicka},
            {Type.SanityDrain, Stat.Category.Sanity},
            {Type.FatigueLoss, Stat.Category.Fatigue}
        };

        [Export] private Vector2I DrainRange { set; get; } = Vector2I.Zero;

        public override void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not Character || target is not Character receiver || 
                    (this.EffectType != Type.SanityDrain && this.EffectType != Type.MagickaDrain && 
                     this.EffectType != Type.FatigueLoss)) {
                return;
            }
            if (DrainEffect.stats.TryGetValue(this.EffectType, out Stat.Category stat)) {
                receiver.Update(stat, -target.Receive(Type.PhysicalHeal, src.Emit(
                    Type.PhysicalHeal, Utilities.Randi(this.DrainRange.X, this.DrainRange.Y)
                )));
            }
        }
    }
}