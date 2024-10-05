using Game.common.characters;
using Game.common.effects;

namespace Game.util.events.battle {
    public class ReceiveEffectEvent : ConditionalEvent<Actor> {
        private readonly EoT effect;
        private readonly int chance;

        public ReceiveEffectEvent(EoT effect, int chance, Actor target) : base(null, [target]) {
            this.effect = effect;
            this.chance = chance;
        }

        public EoT Effect => effect;
        public int Chance => chance;
    }
}