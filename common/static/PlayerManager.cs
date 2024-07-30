using System.Linq;
using Game.common.characters;
using Game.common.characters.race;
using Game.common.characters.skills;
using Game.util;
using Godot;
using Godot.Collections;

namespace Game.common.autoload {
    public partial class PlayerManager : Node {
        private static PlayerManager instance;
        [Export] public int MaxPartySize { set; get; } = 10;
        [Export] public Array<Race> DefaultStartingRaces { set; get; } = [];
        [Export] public Array<Skill> AllPlayerSkills { set; get; } = [];
        private Dictionary<Race.Name, CharacterRandomiser> CharacterRandomisers = [];
        private static readonly Array<PlayerCharacter> combatants = [];
        private static readonly Array<PlayerCharacter> reserves = [];

        public static PlayerManager Instance => instance;
        public static Array<PlayerCharacter> Combatants => combatants;
        public static Array<PlayerCharacter> Reserves => reserves;

        public override void _Ready() {
            PlayerManager.instance = this;
            foreach (Node child in this.GetChildren()) {
                if (child is CharacterRandomiser randomiser) {
                    this.CharacterRandomisers.Add(randomiser.Race, randomiser);
                }
            }
            for (int i = 0; i < this.DefaultStartingRaces.Count; i += 1) {
                PlayerManager.combatants.Add(PlayerCharacter.Random(race: this.DefaultStartingRaces[i]));
            }
        }

        public static string RandomName(Race.Name race, bool female = false) {
            return PlayerManager.instance.CharacterRandomisers[race].RandomName(female);
        }

        public static Texture2D RandomPortrait(Race.Name race, bool isFemale) {
            return PlayerManager.instance.CharacterRandomisers[race].RandomPortrait(isFemale);
        }

        public static Skill[] RandomSkill(int n) {
            if (n > PlayerManager.instance.AllPlayerSkills.Count) {
                return [..PlayerManager.instance.AllPlayerSkills];
            }
            return [..PlayerManager.instance.AllPlayerSkills.Permute().Take(n)];
        }
    }
}