using System;
using Game.common.characters;
using Game.common.characters.enemies;

namespace Game.util.events.party {
    public class DisplayCharacterDetailEvent : EventArgs {
        private readonly PlayerCharacter character;
        private readonly EnemyCharacter enemy;

        public DisplayCharacterDetailEvent(PlayerCharacter character) {
            this.character = character;
        }

        public DisplayCharacterDetailEvent(EnemyCharacter enemy) {
            this.enemy = enemy;
        }

        public PlayerCharacter Character => character;
        public EnemyCharacter Enemy => enemy;
    }
}