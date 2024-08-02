using System;
using System.Collections.Generic;
using Game.common.characters;

namespace Game.util.events.battle {
    public class BattleStartEvent : EventArgs {
        private readonly List<Character> participants;

        public BattleStartEvent(List<Character> participants) {
            this.participants = participants;
        }

        public List<Character> Participants => participants;
    }
}