using Game.common.characters;
using Game.common.stats;

namespace Game.util.events.battle {
    public class AttackedEvent : ConditionalEvent<Actor> {
        private readonly Actor target;
        private readonly ModifiableValue dmg;
        
        public AttackedEvent(Actor target, ModifiableValue dmg) 
                : base(null, [target]) {
            this.target = target;
            this.dmg = dmg;
        }

        public Actor Target => target;
        public ModifiableValue Dmg => dmg;
    }
}