using Game.common.characters;
using Game.common.modifier;
using Godot.Collections;

namespace Game.util.events.battle {
    public class ReceiveModifierEvent : ConditionalEvent<Actor> {
        private readonly Array<Modifier> modifiers;

        public ReceiveModifierEvent(Actor target, Array<Modifier> modifiers) : base(null, [target]) {
            this.modifiers = modifiers;
        }

        public Array<Modifier> Modifiers => modifiers;
    }
}