using System;
using Game.common.characters;
using Game.common.characters.skills;

namespace Game.util.events.battle {
    public class SkillReadyEvent : EventArgs {
        private readonly Skill skill;
        private readonly Actor character;

        public SkillReadyEvent(Skill skill, Actor character) {
            this.skill = skill;
            this.character = character;
        }

        public Skill Skill => skill;
        public Actor Character => character;
    }
}