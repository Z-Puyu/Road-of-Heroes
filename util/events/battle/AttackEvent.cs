using Game.common.characters;
using Game.common.stats;

namespace Game.util.events.battle {
    public class AttackEvent : ConditionalEvent<Actor> {
        private readonly Actor target;
        private readonly StatType dmgType;
        private readonly int minDmg;
        private readonly int maxDmg;
        private readonly int dmgMult;
        private readonly bool isCritical;
        
        public AttackEvent(
            Actor src, Actor target, StatType dmgType, int minDmg, int maxDmg, bool isCritical, 
            int dmgMult = 100
        ) : base(null, [src]) {
            this.target = target;
            this.dmgType = dmgType;
            this.dmgMult = dmgMult;
            this.isCritical = isCritical;
            this.minDmg = minDmg;
            this.maxDmg = maxDmg;
        }

        public Actor Target => target;
        public StatType DmgType => dmgType;
        public int DmgMult => dmgMult;
        public bool IsCritical => isCritical;
        public int MinDmg => minDmg;
        public int MaxDmg => maxDmg;
    }
}