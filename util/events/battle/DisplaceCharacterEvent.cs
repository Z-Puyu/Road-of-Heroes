using System;
using Game.common.characters;

namespace Game.util.events.battle {
    public class DisplaceCharacterEvent : EventArgs {
        private readonly CharacterCard card;
        private readonly int stepSize;
        private readonly CharacterCard target;

        public DisplaceCharacterEvent(CharacterCard card, int stepSize, CharacterCard target) {
            this.card = card;
            this.stepSize = stepSize;
            this.target = target;
        }

        public CharacterCard Card => card;
        public int StepSize => stepSize;
        public CharacterCard Target => target;
    }
}