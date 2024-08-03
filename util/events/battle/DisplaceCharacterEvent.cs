using System;
using Game.common.characters;

namespace Game.util.events.battle {
    public class DisplaceCharacterEvent : EventArgs {
        private readonly CharacterCard card;
        private readonly int stepSize;

        public DisplaceCharacterEvent(CharacterCard card, int stepSize) {
            this.card = card;
            this.stepSize = stepSize;
        }

        public CharacterCard Card => card;
        public int StepSize => stepSize;
    }
}