using Game.common.effects;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(EffectModifier), "", nameof(Resource)), GlobalClass]
    public partial class EffectModifier : Modifier {
        [Export] public Effect.Type TargetEffect { get; set; }
        [Export] public bool ActOnReceive { get; set; }

        public EffectModifier() : base(true) {}

        public override string ToString() {
            string effect = this.TargetEffect switch {
                Effect.Type.MagickaDrain => "Magicka drain",
                Effect.Type.SanityDrain => "Sanity drain",
                Effect.Type.MeleeAttack => "Melee damage",
                Effect.Type.RangedAttack => "Ranged damage",
                Effect.Type.MagicAttack => "Magic damage",
                Effect.Type.PhysicalHeal => "Physical heal",
                Effect.Type.MagicHeal => "Magic heal",
                Effect.Type.FatigueLoss => "Fatigue loss",
                Effect.Type.MagickaRestore => "Magicka gain",
                Effect.Type.SanityRestore => "Sanity gain",
                Effect.Type.FatigueGain => "Fatigue gain",
                _ => ""
            };
            return $"{effect} {(this.ActOnReceive ? "received" : "dealt")} " 
                 + $"{(this.Value >= 0 ? "+" : "")}{this.Value}";
        }
    }
}