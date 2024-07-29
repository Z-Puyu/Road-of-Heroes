using System.Collections.Generic;
using System.Linq;
using Game.common.autoload;
using Game.common.characters;
using Game.common.characters.race;
using Godot;

namespace Game.common.player {
    public partial class Party : Resource {
        [Export] private readonly List<PlayerCharacter> combatants = [null, null, null, null];
        [Export] private readonly List<PlayerCharacter> reserves = [];

        public bool Add(PlayerCharacter character) {
            if (this.combatants.Count(x => x != null) + 
                    this.reserves.Count < GameManager.Instance.MaxPartySize) {
                this.reserves.Add(character);
                return true;
            }
            return false;
        }

        public void Remove(PlayerCharacter character) {
            if (this.combatants.Contains(character)) {
                this.combatants[this.combatants.IndexOf(character)] = null;
            } else {
                this.reserves.Remove(character);
            }
        }

        public bool IsEmpty() {
            return this.combatants.Count(x => x != null) == 0 && this.reserves.Count == 0;
        }

        public void Initialise() {
            if (this.IsEmpty()) {
                this.combatants.Clear();
                foreach (Race race in GameManager.Instance.DefaultStartingRaces) {
                    this.combatants.Add(PlayerCharacter.Random(race: race));
                }
            }
        }
    }
}