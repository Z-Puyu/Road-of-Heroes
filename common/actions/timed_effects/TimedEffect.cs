using Godot;

namespace Game.common.actions {
    public abstract partial class TimedEffect : Resource {
        public enum Type {
            Bleed,
            Blight,
            Poison,
            Burn,
            Frenzy,
            Stun,
            Buff,
            Debuff
        }

        public Type EffectType { protected set; get; }
        public abstract int Duration { protected set; get; }

        protected TimedEffect(Type effectType, int duration) {
            this.EffectType = effectType;
            this.Duration = duration;
        }
    }
}