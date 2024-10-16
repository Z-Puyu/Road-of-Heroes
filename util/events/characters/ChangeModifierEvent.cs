using System;
using Game.common.modifier;

namespace Game.util.events.characters {
    public class ChangeModifierEvent : EventArgs {
        public bool IsRemoving { get; private init; } = false;
        public Modifier Modifier { get; private init; }

        public ChangeModifierEvent(Modifier modifier, bool isRemoving = false) {
            this.Modifier = modifier;
            this.IsRemoving = isRemoving;
        }
    }
}