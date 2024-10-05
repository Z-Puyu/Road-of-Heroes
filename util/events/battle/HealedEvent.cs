using Game.common.characters;
using Game.common.stats;

namespace Game.util.events.battle {
    public class HealedEvent : ConditionalEvent<Actor> {
        private readonly Actor target;
        private readonly Heal heal;

        public HealedEvent(Actor target, Heal heal) : base(null, [target]) {
            this.target = target;
            this.heal = heal;
        }

        public Actor Target => target;
        public Heal Heal => heal;
    }
}