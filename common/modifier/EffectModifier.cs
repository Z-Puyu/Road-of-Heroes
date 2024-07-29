using Game.common.effects;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(EffectModifier), "", nameof(Resource)), GlobalClass]
    public partial class EffectModifier : Modifier {
        [Export] public Effect.Type TargetEffect { get; set; }
        [Export] public bool ActOnReceive { get; set; }

        public EffectModifier() : base(true) {}
    }
}