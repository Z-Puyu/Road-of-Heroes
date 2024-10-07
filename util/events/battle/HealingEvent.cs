using Game.common.characters;
using Game.common.stats;

namespace Game.util.events.battle {
    public class HealingEvent : ConditionalEvent<Actor> {
        private readonly Actor target;
        private readonly ModifiableValueType healTarget;
        private readonly int minHeal;
        private readonly int maxHeal;   
        private readonly bool isCritical;

        public HealingEvent(
            Actor src, Actor target, ModifiableValueType healTarget, int minHeal, int maxHeal, bool isCritical
        ) : base(null, [src]) {
            this.target = target;
            this.healTarget = healTarget;
            this.minHeal = minHeal;
            this.maxHeal = maxHeal;
            this.isCritical = isCritical;
        }

        public Actor Target => target;
        public ModifiableValueType HealTarget => healTarget;
        public int MinHeal => minHeal;
        public int MaxHeal => maxHeal;
        public bool IsCritical => isCritical;
    }
}