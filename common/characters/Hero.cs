using System;
using Game.common.autoload;
using Game.common.characters.classes;
using Game.common.characters.race;
using Game.common.stats;
using Game.util;
using Godot;
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

        public Hero() {}

        private Hero(string name, Level proficiency, Race race) : base(name, null) {
            this.Proficiency = proficiency;
            this.Race = race;
            foreach (Stat s in race.RacialVariations) {
                int val = Util.Randi(s.MinValue, s.MaxValue);
                if (s.Type == StatType.Health || s.Type == StatType.Magicka) {
                    this.Stats.Add(new Stat(s.Type, val, 0, val));
                } else {
                    this.Stats.Add(new Stat(s.Type, val));
                }
            }
            this.Stats.Add(new Stat(StatType.Sanity, 100, 0, 100));
            this.Stats.Add(new Stat(StatType.Stamina, 100, 0, 100));
        }

        public static Hero Random(int level = 0) {
            Level proficiency = (Level)Math.Clamp(level, 0, (int)Level.Legendary);
            return new Hero("New Hero", proficiency, GameManager.RandomRace());
        }

        public override string ToString() {
            return $"{this.Proficiency} {this.Race} {this.Class} {this.Name}";
        }
    }
}