using System.Collections.Generic;
using Game.common.characters;
using Game.common.modifier;

namespace Game.util.events.battle {
    public class ReceiveModifierEvent : ConditionalEvent<Actor> {
        private readonly IEnumerable<Modifier> modifiers;

        public ReceiveModifierEvent(Actor target, IEnumerable<Modifier> modifiers) : base(null, [target]) {
            this.modifiers = modifiers;
        }

        public IEnumerable<Modifier> Modifiers => modifiers;
    }
}