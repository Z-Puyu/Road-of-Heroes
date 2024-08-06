using System;
using Game.common.characters.skills;

namespace Game.util.events.battle {
    public class SkillUnequipedEvent : EventArgs {
        private readonly Skill skill;

        public SkillUnequipedEvent(Skill skill) {
            this.skill = skill;
        }

        public Skill Skill => skill;
    }
}