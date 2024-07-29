using System.Collections.Generic;
using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(SyphonEffect), "", nameof(Resource)), GlobalClass]
    public partial class SyphonEffect : Effect {
        private readonly static Dictionary<Type, Stat.Category> stats = new
                Dictionary<Type, Stat.Category> {
            {Type.MagickaDrain, Stat.Category.Magicka},
            {Type.SanityDrain, Stat.Category.Sanity},
            {Type.FatigueLoss, Stat.Category.Fatigue}
        };
        [Export] private Effect EffectOnTarget { set; get; }
        [Export] private GainEffect SelfGain { set; get; }

        public override void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            this.EffectOnTarget.Apply(src, target, crit);
            this.SelfGain.Apply(src, target);
        }
    }
}