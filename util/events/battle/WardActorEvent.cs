using System;
using Game.common.characters;

namespace Game.util.events.battle {
    public class WardActorEvent : EventArgs {
        public Actor Warder { get; private init; }

        public WardActorEvent(Actor actor) {
            this.Warder = actor;
        }
    }
}