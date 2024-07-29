using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(Effect), "", nameof(Resource)), GlobalClass]
    public abstract partial class Effect : Resource {
        public enum Type {
            MeleeAttack,
            RangedAttack,
            MagicAttack,
            PhysicalHeal,
            MagicHeal,
            MagickaDrain,
            SanityDrain,
            FatigueLoss,
            MagickaRestore,
            SanityRestore,
            FatigueGain,
            Modifier,
            DoT,
            Frenzy,
            HoT
        }

        [Export] public Type EffectType { get; set; }

        public abstract void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false);
    }
}