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
        private readonly Dictionary<Profession, int> ProfessionTendency = [];
        private readonly Race race;
        private Profession profession;
        private readonly List<Skill> activeSkills = [];

        public Race Race => race;
        public string Level => $"LV. {level} " + this.level switch {
            0 => "Novice",
            1 => "Amateur",
            2 => "Apprentice",
            3 => "Professional",
            4 => "Expert",
            5 => "Master",
            6 => "Legendary",
            _ => "",
        };
        public Profession Profession => profession;

        private PlayerCharacter(
            string name, int level, Race race, Texture2D avatar,
            IDictionary<Stat.Category, Stat> stats
        ) : base(name, avatar, stats) {
            this.level = level;
            this.race = race;
        }

        public static PlayerCharacter Random(int level = 0, Race race = null) {
            race ??= GameManager.RandomRace();
            IDictionary<Stat.Category, Stat> stats = Stat.Random();
            race.AdjustStats(stats);
            bool isFemale = Utilities.FairCoin();
            PlayerCharacter character = new PlayerCharacter(
                PlayerManager.RandomName(race.RaceName, isFemale), level, race, 
                PlayerManager.RandomPortrait(race.RaceName, isFemale), stats
            );
            int nRacialSkills = Utilities.Randi(1, 2);
            character.skills.AddRange(race.Skills.Permute().Take(nRacialSkills));
            character.skills.AddRange(PlayerManager.RandomSkill(5 - nRacialSkills));
            character.activeSkills.AddRange(character.skills);
            return character;
        }

        private void EvaluateProfession() {
            foreach (Skill skill in this.skills) {
                foreach (KeyValuePair<Profession, int> pair in skill.ProfessionScores) {
                    if (this.ProfessionTendency.ContainsKey(pair.Key)) {
                        this.ProfessionTendency[pair.Key] += pair.Value;
                    } else {
                        this.ProfessionTendency[pair.Key] = pair.Value;
                    }
                }
            }
            List<Profession> professions = [];
            int highScore = int.MinValue;
            foreach (KeyValuePair<Profession, int> pair in this.ProfessionTendency) {
                if (pair.Value > highScore) {
                    professions.Clear();
                    highScore = pair.Value;
                    professions.Add(pair.Key);
                } else if (pair.Value == highScore) {
                    professions.Add(pair.Key);
                }
            }
            this.profession = professions[Utilities.Randi(0, professions.Count - 1)];
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