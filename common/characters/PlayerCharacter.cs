using System.Collections.Generic;
using System.Linq;
using Game.common.autoload;
using Game.common.characters.profession;
using Game.common.characters.race;
using Game.common.characters.skills;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(PlayerCharacter), "", nameof(Resource)), GlobalClass]
    public partial class PlayerCharacter : Character {
        private int level;
        private readonly Profession profession;
        private readonly Race race;
        private readonly List<Skill> activeSkills = [];

        public Profession Profession => profession;
        public Race Race => race;

        public int Level => level;

        private PlayerCharacter(
            string name, int level, Profession profession, Race race, Texture2D avatar,
            IDictionary<Stat.Category, Stat> stats
        ) : base(name, avatar, stats) {
            this.level = level;
            this.profession = profession;
            this.race = race;
        }

        public static PlayerCharacter Random(int level = 0, Profession profession = null, Race race = null) {
            profession ??= GameManager.RandomProfession();
            race ??= GameManager.RandomRace();
            IDictionary<Stat.Category, Stat> stats = Stat.Random();
            race.AdjustStats(stats);
            bool isFemale = Utilities.FairCoin();
            PlayerCharacter character = new PlayerCharacter(
                GameManager.RandomName(race.RaceName, isFemale), level, profession, race, 
                GameManager.RandomPortrait(race.RaceName, isFemale), stats
            );
            int professionSkills = Utilities.Randi(3, 4);
            character.skills.AddRange(profession.Skills.Permute().Take(professionSkills));
            character.skills.AddRange(race.Skills.Permute().Take(5 - professionSkills));
            character.activeSkills.AddRange(character.skills);
            return character;
        }

        public override string ToString() {
            string proficiency = this.level switch {
                0 => "Novice",
                1 => "Amateur",
                2 => "Apprentice",
                3 => "Professional",
                4 => "Expert",
                5 => "Master",
                6 => "Legendary",
                _ => "",
            };
            return this.name;                                                                                                   
        }
    }
}