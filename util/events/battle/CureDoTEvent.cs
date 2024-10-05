using Game.common.characters;
using Game.common.effects;
using Game.common.effects.eot;

namespace Game.util.events.battle {
    public class CureDoTEvent : ConditionalEvent<Actor> {
        private readonly OverTimeEffect effect;

        public CureDoTEvent(OverTimeEffect effect, Actor target) : base(null, [target]) {
            this.effect = effect;
        }

        public OverTimeEffect Effect => effect;
    }
}