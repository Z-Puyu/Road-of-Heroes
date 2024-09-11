using System;
using Game.common.characters;

namespace Game.util.events.battle {
    public class SkillChangedEvent : EventArgs {
        private readonly PlayerCharacter character;

        public SkillChangedEvent(PlayerCharacter character) {
            this.character = character;
        }

        public PlayerCharacter Character => character;
    }
}