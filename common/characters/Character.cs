using System;
using System.Collections.Generic;
using System.Linq;
using Game.common.autoload;
using Game.common.characters.profession;
using Game.common.characters.race;
using Game.common.characters.skills;
using Game.common.effects;
using Game.common.modifier;
using Game.util;
using Godot;

namespace Game.common.characters {
    [GlobalClass]
    public partial class Character : Resource, IEffectEmitter, IEffectReceiver {
        private readonly IDictionary<Stat.Category, Stat> stats;
        private readonly string name;
        private int level;
        private Profession profession;
        private Race race;
        private readonly List<Skill> skills = [];
        private readonly ModifierManager modifier = new ModifierManager();

        public Character(string name, int level, Profession profession, Race race, IDictionary<Stat.Category, Stat> stats) {
            this.name = name;
            this.level = level;
            this.profession = profession;
            this.race = race;
            this.stats = stats;
        }
        
        public int Get(Stat.Category stat) {
            if (this.stats.TryGetValue(stat, out Stat value)) {
                return Math.Max(this.modifier.Modify(value.Type, value.Value), 0);
            }
            return 0;
        }

        public void Update(Stat.Category stat, int offset) {
            if (this.stats.TryGetValue(stat, out Stat value)) {
                this.stats[stat] = value + offset;
            }
        }

        public int Emit(Effect.Type effect, int value) {
            return Math.Max(0, this.modifier.ModifyOnEmit(effect, value));
        }

        public int Receive(Effect.Type effect, int value) {
            return Math.Max(0, this.modifier.ModifyOnReceive(effect, value));
        }

        public static Character Random(int level = 0, Profession profession = null, Race race = null) {
            profession ??= GameManager.RandomProfession();
            race ??= GameManager.RandomRace();
            IDictionary<Stat.Category, Stat> stats = Stat.Random();
            race.AdjustStats(stats);
            Character character = new Character(
                GameManager.RandomName(race.RaceName), level, profession, race, stats
            );
            int professionSkills = Utilities.Randi(3, 4);
            character.skills.AddRange(profession.Skills.Permute().Take(professionSkills));
            character.skills.AddRange(race.Skills.Permute().Take(5 - professionSkills));
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