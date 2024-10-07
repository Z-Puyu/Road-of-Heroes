using Game.common.effects;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(DoTSuccessChance), "", nameof(Resource)), GlobalClass]
    public partial class DoTSuccessChance : ModifiableValue {
        public DoTSuccessChance(OverTimeEffect effect, int amount) : base(effect switch {
            OverTimeEffect.Bleed => ModifiableValueType.BleedChance,
            OverTimeEffect.Blight => ModifiableValueType.BlightChance,
            OverTimeEffect.Burn => ModifiableValueType.BurnChance,
            OverTimeEffect.Frenzy => ModifiableValueType.FrenzyChance,
            OverTimeEffect.Stun => ModifiableValueType.StunChance,
            _ => ModifiableValueType.StunChance
        }, amount) {}
    }
}