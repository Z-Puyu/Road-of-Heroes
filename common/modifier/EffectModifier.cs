using Game.common.effects;
using Godot;

namespace Game.common.modifier {
    public partial class EffectModifier : Modifier {
        [Export] public Effect.Type TargetEffect { get; set; }
        [Export] public bool ActOnReceive { get; set; }

        public EffectModifier() : base(true) {}
    }
}