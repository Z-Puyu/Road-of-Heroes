using System;
using Game.common.characters;

namespace Game.util.events.battle {
    public class SkillChangedEvent : EventArgs {
        private readonly Actor character;

        public SkillChangedEvent(Actor character) {
            this.character = character;
        }

        public Actor Character => character;
    }
}