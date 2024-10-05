using Game.common.effects;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(DoTSuccessChance), "", nameof(Resource)), GlobalClass]
    public class DoTSuccessChance : Stat {
        public DoTSuccessChance(OverTimeEffect effect, int amount) : base(effect switch {
            OverTimeEffect.Bleed => StatType.BleedChance,
            OverTimeEffect.Blight => StatType.BlightChance,
            OverTimeEffect.Burn => StatType.BurnChance,
            OverTimeEffect.Frenzy => StatType.FrenzyChance,
            OverTimeEffect.Stun => StatType.StunChance,
            _ => StatType.StunChance
        }, amount) {}
    }
}