using System;
using System.Collections.Generic;
using System.Linq;
using Game.common.autoload;
using Game.common.characters.classes;
using Game.common.characters.race;
using Game.common.stats;
using Game.util;
using Game.util.math;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Hero), "", nameof(Resource)), GlobalClass]
    public partial class Hero : Character {
        private enum Level {
            Novice, Amateur, Apprentice, Professional, Expert, Master, Legendary
        }
        [Export] private Level Proficiency { set; get; } = Level.Novice;
        [Export] private Race Race { set; get; }
        [Export] private Class Class { set; get; }
        [Export] private bool IsFemale { set; get; } = false;

        public Hero() {}

        private Hero(
            string name, Level proficiency, Race race, bool isFemale,
            Array<CharacterStat> stats, Array<CharacterAttribute> attributes
        ) : base(name, null, stats, attributes) {
            this.Proficiency = proficiency;
            this.Race = race;
<<<<<<< HEAD
=======
            this.IsFemale = isFemale;
>>>>>>> e50a7f5edd12946b0af396b056629f5c7b368333
        }

        public static Hero Random(uint level = 0) {
            bool isFemale = MathUtil.FairCoin();
            Level proficiency = (Level)Math.Min(level, (int)Level.Legendary);
            Race race = GameManager.RandomRace();
            string name = GameManager.RandomName(race, isFemale);
            List<Stat> baseStats = race.InitRandomStats();
            Array<CharacterStat> stats = [];
            Array<CharacterAttribute> attributes = [];
            baseStats.ForEach(stat => {
                if (stat is CharacterStat s) {
                    stats.Add(s);
                } else if (stat is CharacterAttribute a) {
                    attributes.Add(a);
                }
            });
            return new Hero(name, proficiency, race, isFemale, stats, attributes);
        }

        public override string ToString() {
            return $"{this.Name}: {this.Proficiency} {this.Race.ToAdj()} {(this.Class == null ? "" : $"{this.Class}")}";
        }

        public override void Log() {
            Console.WriteLine(this.ToString());
            base.Log();
        }
    }
}