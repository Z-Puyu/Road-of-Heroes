using Game.common.stats;
using Godot;

namespace Game.common.actions {
    public abstract partial class CombatEffect : Resource {
        public enum Type {
            Bleed = StatType.BleedResist,
            Blight = StatType.BlightResist,
            Poison = StatType.PoisonResist,
            Burn = StatType.BurnResist,
            Frenzy = StatType.FrenzyResist,
            Stun = StatType.StunResist,
            Buff = StatType.BuffResist,
            Debuff = StatType.DebuffResist
        }

        public Type EffectType { protected set; get; }

        protected CombatEffect(Type effectType) {
            this.EffectType = effectType;
        }
    }
}