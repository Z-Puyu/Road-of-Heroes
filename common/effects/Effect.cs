using Godot;

namespace Game.common.effects {
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
            FatigueGain
        }

        [Export] public Type EffectType { get; set; }

        public abstract void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false);
    }
}