using System.Collections.Generic;
using Game.common.characters;
using Game.common.modifier;
using Godot.Collections;

namespace Game.util.events.battle {
    public class RemoveModifierEvent : ConditionalEvent<Actor> {
        private readonly IEnumerable<Modifier> modifiers;

        public RemoveModifierEvent(
            Actor target, IEnumerable<Modifier> modifiers
        ) : base(null, [target]) {
            this.modifiers = modifiers;
        }

        public IEnumerable<Modifier> Modifiers => modifiers;
    }
}