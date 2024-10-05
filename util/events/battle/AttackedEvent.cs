using Game.common.characters;
using Game.common.stats;

namespace Game.util.events.battle {
    public class AttackedEvent : ConditionalEvent<Actor> {
        private readonly Actor target;
        private readonly Stat dmg;
        
        public AttackedEvent(Actor target, Stat dmg) 
                : base(null, [target]) {
            this.target = target;
            this.dmg = dmg;
        }

        public Actor Target => target;
        public Stat Dmg => dmg;
    }
}